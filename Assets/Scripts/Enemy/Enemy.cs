using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public delegate void myAction();
public delegate void myAction<T>(T t);

public class Enemy : CharacterProperty
{
    public GameObject[] myDropItem; //드롭 아이템
    public int[] DropPersent; //드롭 아이템 확률
    public enum STATE { Create, Alive, Battle, Death };
    protected enum TYPE { Normal, Boss, Special };
    public NavMeshAgent myNav;
    Coroutine coMove;
    protected float MoveSpeed = 3.0f;
    protected float RotSpeed = 360.0f;
    protected NavMeshPath myPath;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    protected void FollowPlayer(Transform myTarget, float Range, myAction done = null)
    {
        if (NavMesh.CalculatePath(transform.position, myTarget.position, 1 << NavMesh.GetAreaFromName("Walkable"), myPath))
        {
            if (coMove != null) StopCoroutine(coMove);
            if (coRot != null) StopCoroutine(coRot);
            coMove = StartCoroutine(Moving(myPath.corners, Range, done));
        }
    }

    Coroutine coRot = null;
    protected IEnumerator Moving(Vector3[] poslist, float Range, myAction done = null)
    {
        if (poslist.Length <= 1) yield break;        
        int nextPos = 1;
        myAnim.SetBool("isMoving", true);
        while (nextPos < poslist.Length)
        {
            Vector3 dir = poslist[nextPos] - poslist[nextPos - 1];
            float dist = dir.magnitude;
            dir.Normalize();

            if (coRot != null) StopCoroutine(coRot);
            coRot = StartCoroutine(Movement1.Rotating(transform, poslist[nextPos], RotSpeed));

            while (dist > Range)
            {
                float delta = Time.deltaTime * MoveSpeed;
                if (delta > dist)
                {
                    delta = dist;
                }
                dist -= delta;
                transform.Translate(dir * delta, Space.World);
                yield return null;
            }
            nextPos++;
        }
        myAnim.SetBool("isMoving", false);
        done?.Invoke();
    }
    protected IEnumerator RandMove(Vector3 RandStartPos,float MoveSpeed)
    {
        yield return new WaitForSeconds(Random.Range(2, 3));
        Vector3 Pos = RandStartPos;
        Pos.x = Pos.x + Random.Range(-10, 11);
        Pos.z = Pos.z + Random.Range(-10, 11);
        MoveToPosition(Pos, MoveSpeed, 360.0f ,() => StartCoroutine(RandMove(RandStartPos, MoveSpeed)));
    }
    protected void MoveToPosition(Vector3 targetPos, float MovSpeed = 1.0f, float RotSpeed = 360.0f, MyAction done = null)
    {
        //if (Vector3.Distance(targetPos, transform.position) < 0.01f)
        //{
        //    done?.Invoke();
        //    return;
        //}
        if (coMove != null) StopCoroutine(coMove);
        coMove = StartCoroutine(MovingToPosition(targetPos, MovSpeed, done));
        if (coRot != null) StopCoroutine(coRot);
        coRot = StartCoroutine(Rotating(transform, targetPos, RotSpeed));
    }

    IEnumerator MovingToPosition(Vector3 target, float MovSpeed, MyAction done = null)
    {
        Vector3 dir = target - transform.position;
        float dist = dir.magnitude;
        if (dist <= Mathf.Epsilon) yield break;
        dir.Normalize();

        myAnim.SetBool("isMoving", true);

        while (dist > 0.0f)
        {
            if (!myAnim.GetBool("isAttacking"))
            {
                float delta = MovSpeed * Time.deltaTime;
                if (delta > dist)
                {
                    delta = dist;
                }
                dist -= delta;
                transform.Translate(dir * delta, Space.World);
            }
            else
            {
                break;
            }
            yield return null;
        }
        myAnim.SetBool("isMoving", false);
        //if (done != null) done();
        done?.Invoke();
    }
    public static IEnumerator Rotating(Transform transform, Vector3 target, float RotSpeed)
    {
        Vector3 dir = target - transform.position;
        if (dir.magnitude <= Mathf.Epsilon) yield break;
        dir.Normalize();
        float d = Vector3.Dot(dir, transform.forward);
        float r = Mathf.Acos(d);
        float angle = r * Mathf.Rad2Deg;//Mathf.Rad2Deg = 180.0f / Mathf.PI;

        if (angle > Mathf.Epsilon)
        {
            float rotDir = Vector3.Dot(dir, transform.right) < 0.0f ? -1.0f : 1.0f;
            while (angle > Mathf.Epsilon)
            {
                float delta = RotSpeed * Time.deltaTime;
                if (delta > angle)
                {
                    delta = angle;
                }
                angle -= delta;
                transform.Rotate(Vector3.up * rotDir * delta, Space.World);
                yield return null;
            }
        }
    }

    protected void DropNormalItem()
    {
        if (myDropItem == null) return;
        for(int i = 0; i < myDropItem.Length; i++)
        {
            float per = Random.Range(1, 101);
            if(per > 0 && per <= DropPersent[i])
            {
                Dropping(i);
            }
        }
    }
    protected void Dropping(int a, int power = 2)
    {
        GameObject obj = Instantiate(myDropItem[a], transform.position + Vector3.up*2, Quaternion.identity, null);
        obj.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-5, 5) * (power / 2), 5 * power
            , Random.Range(-5, 5) * (power / 2)), ForceMode.Impulse);
    }
}
