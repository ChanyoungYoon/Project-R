using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Timers;

public class BossScript : MonoBehaviour
{
    public Transform other;
    public GameObject pattern3HitBox;
    public GameObject poisonSmog;
    public GameObject hitCheck;
    public GameObject wallHitCheck;
    public BoxCollider hitBox;
    public BoxCollider wallHitBox;
    public bool isAttack;
    public bool isHit;
    public bool isDelay;
    public bool pattern3Repeating = false;
    public bool pattern4Repeating = false;
    public bool isPoisoned;
    private bool attackCooltime = false;
    public int damage = 2;
    public Health health;
    public Health myHealth;

    List<GameObject> smogs = new List<GameObject>();
    Rigidbody rigid;
    BoxCollider boxCollider;
    NavMeshAgent nav;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        nav = GetComponent<NavMeshAgent>();
        isAttack = false;
        health = GameObject.Find("Player").GetComponent<Health>();
        myHealth = this.GetComponent<Health>();
    }


    void Update()
    {

        if (health.isDie() == false)
        {
            nav.SetDestination(other.position);

        }
        else
        {
            nav.isStopped = true;
            rigid.velocity = Vector3.zero;
        }


    }

    private void FixedUpdate()
    {
        //현재 체력에 따라 패턴을 다르게 적용
        if ((myHealth.isDie() == false) && (isAttack == false) && myHealth.CurrentHealth >= myHealth.MaxHealth * 0.75)
        {
            
            Pattern1();
        }

        else if ((myHealth.isDie() == false) && (isAttack == false) && myHealth.CurrentHealth >= myHealth.MaxHealth * 0.5)
        {
            Pattern2();
        }

        else if ((myHealth.isDie() == false) && (isAttack == false) && myHealth.CurrentHealth >= myHealth.MaxHealth * 0.25)
        {
            if (pattern3Repeating == false)
            {
                InvokeRepeating("Pattern3", 1, 10);
                pattern3Repeating = true;
            }
        }

        else if ((myHealth.isDie() == false) && (isAttack == false) && myHealth.CurrentHealth >= myHealth.MaxHealth * 0)
        {
            //패턴 쿨타임 지정(플래그변수 이용)
            CancelInvoke("Pattern3");
            if (pattern4Repeating == false)
            {
                InvokeRepeating("Pattern4", 1, 5);
                pattern4Repeating = true;
            }
            else if (Smog.inPoisonArea == true)
            {
                Pattern1();
            }
        }
        else if(myHealth.isDie() == true)
        {
            CancelInvoke("Pattern4");
            print("보스 사망");
        }
    }

    //콜라이더 접촉시 데미지 주는 함수
    public void OnTriggerStay(Collider other)
    {
        if (attackCooltime == false && other.tag == "Player")
        {
            StartCoroutine(GiveDamage(other, damage));
        }
    }

    IEnumerator GiveDamage(Collider collider, int damage)
    {
        attackCooltime = true;
        Global.EnemyAttack(collider, damage);
        yield return new WaitForSeconds(1f);
        attackCooltime = false;
    }



    //적에게 돌진하는 패턴 1을 레이캐스트로 구현
    private void Pattern1()
    {
        float targetRadius = 0.7f;
        float targetRange = 2f;

        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange,
            LayerMask.GetMask("Player"));

        if (rayHits.Length > 0 && isAttack == false)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        nav.isStopped = true;
        isAttack = true;
        Debug.Log("Attack()");

        yield return new WaitForSeconds(0.2f);
        rigid.AddForce(transform.forward * 5, ForceMode.Impulse);
        hitBox.enabled = true;
        hitCheck.SetActive(true);
        //돌진하는 중간에 벽에 충돌하는지 체크
        InvokeRepeating("HitCheck", 0, 0.1f);

        
        yield return new WaitForSeconds(2f);
        CancelInvoke();
        wallHitBox.enabled = false;
        wallHitCheck.SetActive(false);

        yield return new WaitForSeconds(1f);
        hitBox.enabled = false;
        hitCheck.SetActive(false);
        rigid.velocity = Vector3.zero;
        nav.isStopped = false;
        isAttack = false;
        pattern4Repeating = false;
        isHit = false;
    }

    

    void HitCheck()
    {
        RaycastHit hit;
        float targetRadius = 0.1f;
        float maxDistance = 0.5f;
        isHit = false;

        isHit =
    Physics.SphereCast(transform.position, targetRadius, transform.forward, out hit, maxDistance, LayerMask.GetMask("Wall"));


        if (isHit)
        {
            CancelInvoke();
            rigid.velocity = Vector3.zero;
            hitBox.enabled = false;
            hitCheck.SetActive(false);
            wallHitBox.enabled = true;
            wallHitCheck.SetActive(true);
            print("벽과 충돌");
        }

    }

    //패턴2는 패턴1과 거의 동일
    private void Pattern2()
    {
        float targetRadius = 0.7f;
        float targetRange = 2f;

        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange,
            LayerMask.GetMask("Player"));

        if (rayHits.Length > 0 && isAttack == false)
        {
            StartCoroutine(Attack2());
        }

    }

    IEnumerator Attack2()
    {
        nav.isStopped = true;
        isAttack = true;
        Debug.Log("Attack2()");

        yield return new WaitForSeconds(0.2f);
        wallHitBox.enabled = true;
        wallHitCheck.SetActive(true);
        
        yield return new WaitForSeconds(0.8f);
        wallHitBox.enabled = false;
        wallHitCheck.SetActive(false);
        rigid.AddForce(transform.forward * 5, ForceMode.Impulse);
        hitBox.enabled = true;
        hitCheck.SetActive(true);
        InvokeRepeating("HitCheck", 0, 0.1f);


        yield return new WaitForSeconds(2f);
        CancelInvoke();
        wallHitBox.enabled = false;
        wallHitCheck.SetActive(false);

        yield return new WaitForSeconds(1f);
        rigid.velocity = Vector3.zero;
        nav.isStopped = false;
        isAttack = false;
    }


    //패턴3 구현
    public void Pattern3()
    {
        StartCoroutine(Attack3());
    }

    
    IEnumerator Attack3()
    {
        nav.isStopped = true;
        isAttack = true;
        Debug.Log("Attack3()");
        yield return new WaitForSeconds(1.0f); //머리 찍기 전 피할시간 주기

        wallHitBox.enabled = true;
        wallHitCheck.SetActive(true);
        yield return new WaitForSeconds(1.5f); //머리를 찍는 동작

        wallHitBox.enabled = false;
        wallHitCheck.SetActive(false);
        randomMeteo(); 
        yield return new WaitForSeconds(2.0f);

        nav.isStopped = false;

        yield return new WaitForSeconds(0.1f);
        isAttack = false;

    }


    //낙석패턴 구현, 랜덤한 위치를 구해내는 함수 randomPosition을 만듦.
    private void randomMeteo()
    {
        pattern3HitBox.SetActive(true);
        GameObject[] fallingRocks = new GameObject[16];
        for (int i = 0; i < 10; i++)
        {
            fallingRocks[i] = Instantiate(pattern3HitBox);
            fallingRocks[i].transform.position = randomPosition(gameObject.transform.position);
            fallingRocks[i].name = "fallingRocks";
            Destroy(fallingRocks[i], 2f);
        }
        pattern3HitBox.SetActive(false);
    }

    //상하좌우 1칸씩 이동, 그자리에 물체가 있을경우 원위치. 이 작업을 n번 반복함.
    //보스방 생성시 고정좌표 랜덤추첨 방식으로 변경 예정
    private Vector3 randomPosition(Vector3 Position)
    {
        Vector3 rememberPosition = Position;
        float targetRadius = 0.1f;
        float targetRange = 0.1f;
        for (int i = 0; i < 40; i++)
        {
            int randomNumber = Random.Range(1, 5);
            if (randomNumber == 1)
            {
                Position = Position + new Vector3(0, 0, -1);
            }
            if (randomNumber == 2)
            {
                Position = Position + new Vector3(0, 0, 1);
            }
            if (randomNumber == 3)
            {
                Position = Position + new Vector3(-1, 0, 0);
            }
            if (randomNumber == 4)
            {
                Position = Position + new Vector3(1, 0, 0);
            }

            pattern3HitBox.transform.position = Position;
            
            int layerMask = (1 << LayerMask.NameToLayer("Floor"));  // Everything에서 Player 레이어만 제외하고 충돌 체크함
            layerMask = ~layerMask;

            RaycastHit[] rayHits =
            Physics.SphereCastAll(pattern3HitBox.transform.position, targetRadius, transform.forward, targetRange, layerMask);
            if (rayHits.Length > 0)
            {
                Position = rememberPosition;
            }
            else
            {
                rememberPosition = Position;
            }
        }

        return Position;
    }


    //패턴4는 독안개 지역을 구현하는 Smog.cs파일을 따로 만듦
    private void Pattern4()
    {
        StartCoroutine(PoisonSmog());
    }


    IEnumerator PoisonSmog()
    {
        nav.isStopped = true;
        isAttack = true;
        yield return new WaitForSeconds(0.2f);

        if (smogs.Count < 7)
        {
            smogs.Add(Instantiate(poisonSmog, transform.position + transform.forward * 1.5f, new Quaternion()));
            Invoke("RemoveList", 9.5f);
            yield return new WaitForSeconds(1f);
        }

        nav.isStopped = false;
        isAttack = false;
    }

    void RemoveList()
    {
        Destroy(smogs[0]);
        smogs.RemoveAt(0);
    }


    /*

        


        IEnumerator OnDamage(Vector3 reactVec)
        {

            culHealth -= 1;
            Debug.Log(culHealth);

            if (culHealth > 0)
            {
                reactVec = reactVec.normalized;
                rigid.AddForce(reactVec * 2, ForceMode.Impulse);
                yield return new WaitForSeconds(0.2f);
                rigid.velocity = new Vector3(0, 0, 0);
            }
            else
            {
                gameObject.layer = 7;
                Destroy(gameObject, 2); //2초뒤 삭제를 이렇게 구현 가능(하지만 시체도 데미지가 들어간다.

            }
            yield return new WaitForSeconds(1.0f);
            isDelay = false;
        }

    IEnumerator WallAttack()
    {
        nav.isStopped = true;
        rigid.velocity = Vector3.zero;
        isAttack = true;
        wallCollisionCheck = true;
        Debug.Log("WallAttack()");
        wallHitBox.enabled = true;
        yield return new WaitForSeconds(1.0f);

        wallHitBox.enabled = false;
        nav.isStopped = false;
        yield return new WaitForSeconds(1.0f);

        
        wallCollisionCheck = false;
        yield return new WaitForSeconds(0.1f);
        isAttack = false;
    }

    
    private void WallCollision()
    {

        float targetRadius = 1f;
        float targetRange = 0f;

        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange,
            LayerMask.GetMask("Wall"));

        if(rayHits.Length > 0 && wallCollisionCheck == false)
        {
            StartCoroutine(WallAttack());   
        }
    }
        */
}

/*
 * https://www.youtube.com/watch?v=LUnZHdcOe_M&ab_channel=%EA%B3%A8%EB%93%9C%EB%A9%94%ED%83%88 골드메탈 몬스터 만들기

 */ 