using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    public enum State : int
    {
        None = -1,  //사용전
        Ready = 0,  //준비 완료
        Appear,     //등장
        Battle,     //전투중
        Dead,       //사망
        Disappear,  //퇴장
    }

    [SerializeField]
    State CurrentState = State.None;

    /// <summary>
    /// 최고 속도
    /// </sumary>
    const float MaxSpeed = 10.0f;

    /// <summary>
    /// 최고 속도에 이르는 시간
    /// </sumary>
    const float MaxSpeedTime = 0.5f;

    /// <summary>
    /// 목표점
    /// </sumary>
    [SerializeField]
    Vector3 TargetPosition;

    [SerializeField]
    float CurrentSpeed;

    /// <summary>
    /// 방향을 고려한 속도 벡터
    /// </sumary>
    Vector3 CurrentVelocity;

    float MoveStartTime = 0.0f; // 이동시작 시간

    // float BattleStartTime = 0.0f;

    [SerializeField]
    Transform FireTransform;

    [SerializeField]
    GameObject Bullet;

    [SerializeField]
    float BulletSpeed = 1;

    float LastBattleUpdateTime = 0.0f;

    [SerializeField]
    int FireRemainCount = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    // void Update()
    protected override void UpdateActor()
    {
        // if(Input.GetKeyDown(KeyCode.L))
        // {
        //     Appear(new Vector3(7.0f, transform.position.y, transform.position.z));
        // }

        switch(CurrentState)
        {
            case State.None:
            case State.Ready:
                break;
            case State.Dead:
                break;
            case State.Appear:
            case State.Disappear:
                UpdateSpeed();
                UpdateMove();
                break;
            case State.Battle:
                UpdateBattle();
                break;
            default:
                Debug.LogError("Undefined State!");
                break;
        }

        // if(Input.GetKeyDown(KeyCode.K))
        // {
        //     Disappear(new Vector3(-15.0f, 0.0f, 0.0f));
        // }

        // if(CurrentState == State.Appear || CurrentState == State.Disappear)
        // {
        //     UpdateSpeed();
        //     UpdateMove();
        // }

    }

    void UpdateSpeed()
    {
        CurrentSpeed = Mathf.Lerp(CurrentSpeed, MaxSpeed, (Time.time - MoveStartTime)/MaxSpeedTime);
    }

    void UpdateMove()
    {
        float distance = Vector3.Distance(TargetPosition, transform.position);
        if(distance == 0)
        {
            Arrived();
            return;
        }

        CurrentVelocity = (TargetPosition - transform.position).normalized * CurrentSpeed;

        // 속도 = 거리 / 시간 이므로 시간 = 거리 / 속도
        transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref CurrentVelocity, distance / CurrentSpeed, MaxSpeed);
    }

    void Arrived()
    {
        CurrentSpeed = 0.0f;
        if(CurrentState == State.Appear)
        {
            CurrentState = State.Battle;
            LastBattleUpdateTime = Time.time;
        }
        else if (CurrentState == State.Disappear)
        {
            CurrentState = State.None;
        }
    }

    public void Appear(Vector3 targetPos)
    {
        TargetPosition = targetPos;
        CurrentSpeed = MaxSpeed;

        CurrentState = State.Appear;
        MoveStartTime = Time.time;
    }

    void Disappear(Vector3 targetPos)
    {
        TargetPosition = targetPos;
        CurrentSpeed = 0.0f;

        CurrentState = State.Disappear;
        MoveStartTime = Time.time;
    }

    void UpdateBattle()
    {
        if(Time.time - LastBattleUpdateTime > 1.0f)
        {
            if(FireRemainCount > 0)
            {
                Fire();
                FireRemainCount--;
            }
            else{
                Disappear(new Vector3(-15.0f, transform.position.y, transform.position.z));
            }

            LastBattleUpdateTime = Time.time;
        }
    }

    private void OnTriggerEnter(Collider other) {
        // Debug.Log("other=" + other.name);

        Player player = other.GetComponentInParent<Player>();
        if(player)
            player.OnCrash(this);
    }

    public void OnCrash(Player player)
    {
        Debug.Log("OnCrash enemy = " + player);
    }

        public void Fire()
    {
        GameObject go = Instantiate(Bullet);
        
        Bullet bullet = go.GetComponent<Bullet>();
        bullet.Fire(OwnerSide.Enemy, FireTransform.position, -FireTransform.right, BulletSpeed, Damage);
    }
}
