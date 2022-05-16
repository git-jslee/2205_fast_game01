using UnityEngine;

public class Player : Actor
{
    /// <summary>
    /// 이동할 벡터
    /// </sumary>
    [SerializeField]
    Vector3 MoveVector = Vector3.zero;

    /// <summary>
    /// 이동 속도
    /// </sumary>
    [SerializeField]
    float Speed;

    [SerializeField]
    BoxCollider boxCollider;

    [SerializeField]
    Transform MainBGQuadTransform;

    [SerializeField]
    Transform FireTransform;

    [SerializeField]
    GameObject Bullet;

    [SerializeField]
    float BulletSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    // void Update()
    protected override void UpdateActor()
    {
        UpdateMove();
    }

    /// <summary>
    /// 이동벡터에 맞게 위치를 변경
    /// </sumary>
    void UpdateMove()
    {
        if(MoveVector.sqrMagnitude == 0)
            return;

        MoveVector = AdjustMoveVector(MoveVector);

        transform.position += MoveVector;
    }

    public void ProcessInput(Vector3 moveDirection)
    {
        MoveVector = moveDirection * Speed * Time.deltaTime;
    }

    Vector3 AdjustMoveVector(Vector3 moveVector)
    {
        Vector3 result = Vector3.zero;

        result = boxCollider.transform.position + boxCollider.center + moveVector;

        if(result.x - boxCollider.size.x * 0.5f < -MainBGQuadTransform.localScale.x * 0.5f)
            moveVector.x = 0;

        if(result.x - boxCollider.size.x * 0.5f > MainBGQuadTransform.localScale.x * 0.5f)
            moveVector.x = 0;
        
        if(result.y - boxCollider.size.x * 0.5f < -MainBGQuadTransform.localScale.y * 0.5f)
            moveVector.y = 0;

        if(result.y - boxCollider.size.x * 0.5f > MainBGQuadTransform.localScale.y * 0.5f)
            moveVector.y = 0;

        return moveVector;
    }

    private void OnTriggerEnter(Collider other) {
        // Debug.Log("other = " + other.name);

        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy)
        {
            if(!enemy.IsDead)
                enemy.OnCrash(this, CrashDamage);
        }
            
    }

    public override void OnCrash(Actor attacker, int damage)
    {
        Debug.Log("OnCrash player = " + attacker);
        base.OnCrash(attacker, damage);
    }

    public void Fire()
    {
        GameObject go = Instantiate(Bullet);
        
        Bullet bullet = go.GetComponent<Bullet>();
        bullet.Fire(this, FireTransform.position, FireTransform.right, BulletSpeed, Damage);
    }

    protected override void OnDead(Actor killer)
    {
        base.OnDead(killer);
        gameObject.SetActive(false);
    }
}
