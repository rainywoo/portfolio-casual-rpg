using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : CharacterProperty
{
    protected enum STATE { Create, Alive, Battle, Death };
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



    protected void FollowPlayer(Transform myTarget, float Range)
    {
        if (NavMesh.CalculatePath(transform.position, myTarget.position, 1 << NavMesh.GetAreaFromName("Walkable"), myPath))
        {
            if (coMove != null) StopCoroutine(coMove);
            if (coRot != null) StopCoroutine(coRot);
            coMove = StartCoroutine(Moving(myPath.corners, Range));
        }
    }

    Coroutine coRot = null;
    protected IEnumerator Moving(Vector3[] poslist, float Range)
    {
        if (poslist.Length <= 1) yield break;        
        int nextPos = 1;
        myAnim.SetBool("isMoving", true);
        while (nextPos < poslist.Length)
        {
            Vector3 dir = poslist[nextPos] - poslist[nextPos - 1];
            float dist = dir.magnitude - Range;
            dir.Normalize();

            if (coRot != null) StopCoroutine(coRot);
            coRot = StartCoroutine(Movement1.Rotating(transform, poslist[nextPos], RotSpeed));

            while (dist > Mathf.Epsilon)
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
    }
}
