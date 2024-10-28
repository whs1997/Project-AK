using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class HYJ_Eneme : MonoBehaviour
{
    [SerializeField] GameObject monster;
    [SerializeField] MonsterType monsterType;
    [SerializeField] MonsterAttackType monsterAttackType;
    [SerializeField] float monsterAttackPower;
    [SerializeField] float monsterAttackRange;
    [SerializeField] float setBossHp;


    [SerializeField] float monsterHp;
    [SerializeField] float monsterMoveSpeed;
    [SerializeField] float playerDistance;

    [SerializeField] Animator monsterAnimator;

    //------------------------임의 변수---------------------------//
    public float playerAttackPower=20;

    public enum MonsterType
    {
        Nomal,
        Elite,
        Boss
    }

    public enum MonsterAttackType
    {
        shortAttackRange,
        longAttackRange
    }

    void Start()
    {
        MonsterSetHp();
        MonsterSetAttackRange();
    }

    void Update()
    {
        MonsterDie();
        MonsterMover();
    }

    // Comment : 몬스터타입에 따라 몬스터의 체력을 조정한다.
    private void MonsterSetHp()
    {
        if (monsterType == MonsterType.Boss) // Comment : 몬스터의 타입이 Boss라면 설정한 BossHp로 Hp가 설정한다.
        {
            monsterHp = setBossHp;
        }
        else // Comment : Boss가 아닌 Nomal, Elite 몬스터는 Hp가 100으로 설정한다.
        {
            monsterHp = 100;
        }
    }

    // Comment : 몬스터 공격범위를 조정한다.
    private void MonsterSetAttackRange()
    {
        if(monsterAttackType == MonsterAttackType.shortAttackRange) // Comment : 근거리 타입이라면 공격범위를 3으로 설정한다.
        {
            monsterAttackRange = 3;
        }
        else if(monsterAttackType == MonsterAttackType.longAttackRange) // Commnet : 원거리 타입이라면 공격 범위를 7로 설정한다.
        { 
            monsterAttackRange = 7;
        }
    }

    // Comment : Player 태그의 오브젝트를 찾고 해당 오브젝트로 Monster가 이동한다.
    private void MonsterMover()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // TODO : 태그탐색을 레이어로 변경시키기!
        if (player != null)
        {
            playerDistance = Vector3.Distance(monster.transform.position, player.transform.position); // Comment : 플레이어와 몬스터의 거리

            if (playerDistance > monsterAttackRange) // Comment : 플레이어와의 거리가 공격범위 밖일 때
            {
                Debug.Log("이동 중");
                monsterAnimator.SetBool("Run Forward",true);
                monster.transform.position = Vector3.MoveTowards(monster.transform.position, new Vector3(player.transform.position.x,0,player.transform.position.z), monsterMoveSpeed/50);
                monster.transform.LookAt(new Vector3(player.transform.position.x,0,player.transform.position.z)); // Comment : 몬스터 
            }
            else // Comment : 플레이어가 몬스터의 공격범위로 들어왔을 때
            {
                MonsterAttack();
            }
        }
    }

    // Comment : 온트리거 엔터를 이용하여 총알과의 충돌 여부를 확인, 충돌 시, 캐릭터의 공격력 or 무기의 공격력이 완료되면 몬스터 피격 함수를 진행시킨다.
    private void MonsterTakeMamage()
    {
        // TODO : 무기의 공격력과 총알 구현이 완료되면 몬스터 피격 함수를 진행시킨다.
        // Comment : 현재는 임의로 playerAttackPower 변수를 활용하여 작성했다.
        if (monsterType == MonsterType.Nomal)
        {
            monsterHp -= playerAttackPower;
        }
        else if(monsterType == MonsterType.Elite)
        {
            if(playerAttackPower-15 > 0)
            {
                monsterHp -= playerAttackPower - 15;
            }
        }
    }

    //Comment : 몬스터가 플레이어를 공격 시의 함수
    private void MonsterAttack()
    {
        if (monsterHp > 0)
        {
            monsterAnimator.SetTrigger("Pound Attack");
            Debug.Log("몬스터가 공격!");
            // TODO : 공격 딜레이 만들기

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet")){
            MonsterTakeMamage();
            
        }
    }

    // Comment : 몬스터 사망
    private void MonsterDie()
    {
        if(monsterHp <= 0) // Comment : 몬스터의 Hp가 0이 되면 몬스터 오브젝트를 삭제한다.
        {
            Debug.Log("몬스터 사망");
            //TODO : 몬스터 Die 애니메이션 실행
            monsterAnimator.SetTrigger("Die");
            Destroy(gameObject.GetComponent<BoxCollider>());
            Destroy(gameObject.GetComponent<Rigidbody>());
            Destroy(gameObject,2f);
        }
    }
}
