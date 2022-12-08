using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void MyAction();
public delegate void MyAction<T>(T t);


public class Movement1 : MonoBehaviour
{

    Animator _anim = null;
    protected Animator myAnim
    {
        get
        {
            if (_anim == null)
            {
                _anim = GetComponent<Animator>();
                if (_anim == null)
                {
                    _anim = GetComponentInChildren<Animator>();
                }
            }
            return _anim;
        }
    }

    Coroutine coMove = null;
    Coroutine coRot = null;

    protected void MoveToPosition(Vector3 targetPos, float MovSpeed = 1.0f, float RotSpeed = 360.0f, MyAction done = null)
    {
        if (Vector3.Distance(targetPos, transform.position) < 0.01f)
        {
            done?.Invoke();
            return;
        }
        if (coMove != null) StopCoroutine(coMove);
        coMove = StartCoroutine(MovingToPosition(targetPos, MovSpeed, done));
        if (coRot != null) StopCoroutine(coRot);
        coRot = StartCoroutine(Rotating(transform, targetPos, RotSpeed));
    }

    public static IEnumerator Rotating(Transform transform, Vector3 target, float RotSpeed)
    {
        Vector3 dir = target - transform.position;
        if (Mathf.Approximately(dir.magnitude,0.0f)) yield break;
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

    IEnumerator MovingToPosition(Vector3 target, float MovSpeed, MyAction done)
    {
        Vector3 dir = target - transform.position;
        float dist = dir.magnitude;
        if (dist <= Mathf.Epsilon) yield break;
        dir.Normalize();

        myAnim.SetBool("IsMoving", true);

        while (dist > 0.0f)
        {
            if (!myAnim.GetBool("IsAttacking"))
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
        myAnim.SetBool("IsMoving", false);
        //if (done != null) done();
        done?.Invoke();
    }

    protected void FollowTarget(Transform target, float MovSpeed = 1.0f, float RotSpeed = 360.0f, MyAction reached = null)
    {
        if (coMove != null) StopCoroutine(coMove);
        coMove = StartCoroutine(FollowingTarget(target, MovSpeed, RotSpeed, reached));
        if (coRot != null) StopCoroutine(coRot);
    }

    IEnumerator SimpleFollowing(Transform target, float MovSpeed, float RotSpeed, MyAction reached)
    {
        float AttackRange = 1.1f;
        while (target != null)
        {
            //transform.LookAt(target);
            Vector3 rot = Vector3.RotateTowards(transform.forward, target.position - transform.position, RotSpeed * Mathf.Deg2Rad * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(rot);
            if (Vector3.Distance(transform.position, target.position) > AttackRange)
            {
                myAnim.SetBool("IsMoving", true);
                float delta = MovSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, target.position, delta);
            }
            else
            {
                myAnim.SetBool("IsMoving", false);
                reached?.Invoke();
            }
            yield return null;
        }
    }

    IEnumerator FollowingTarget(Transform target, float MovSpeed, float RotSpeed, MyAction reached)
    {
        float AttackRange = 1.1f;
        while (target != null)
        {
            //transform.LookAt(target.position);
            Vector3 dir = target.position - transform.position;
            dir.y = 0.0f;
            float dist = dir.magnitude;

            Vector3 rot = Vector3.RotateTowards(transform.forward, dir, RotSpeed * Mathf.Deg2Rad * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(rot);

            if (!myAnim.GetBool("IsAttacking") && dist > AttackRange + 0.01f)
            {
                myAnim.SetBool("IsMoving", true);
                dir.Normalize();
                float delta = MovSpeed * Time.deltaTime;
                if (delta > dist - AttackRange)
                {
                    delta = dist - AttackRange;
                    myAnim.SetBool("IsMoving", false);
                }
                transform.Translate(dir * delta, Space.World);
            }
            else
            {
                myAnim.SetBool("IsMoving", false);
                reached?.Invoke();
            }
            yield return null;
        }
    }
}