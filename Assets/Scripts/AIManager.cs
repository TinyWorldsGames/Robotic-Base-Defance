using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AIManager : MonoBehaviour
{

    [SerializeField]
    Transform injuredSoldiersParent;

    public Transform target,soldierPosition;

    public BoxCollider fullCollider, smallCollider;

    public GameObject player;

    public NavMeshAgent agent;

    public List<GameObject> enemies = new List<GameObject>();

    public Animator animator;

    [SerializeField]
    float fireSpeed;

    [SerializeField]
    Transform firePoint;
    [SerializeField]
    GameObject bullet;

   
    public float health=100;


    public bool isActiveFire;

    public bool isHaveTarget;

    [SerializeField]
    BodyPartManager bodyPartManager;

    [SerializeField]
    Gun[] guns;

    public int id;

    [SerializeField]
    GameObject robotMesh;
   
    [HideInInspector]
     public string targetName;
    

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void GotoTarget(Transform point, string tartgetName)
    {
        this.targetName = tartgetName;

        agent.SetDestination(point.position);

        agent.isStopped = false;

        animator.SetBool("isRun", true);

    }

    void CheckArrive()
    {
        if (agent.hasPath && agent.remainingDistance <0.1f)
        {
            animator.SetBool("isRun", false);

            agent.isStopped = true;

            target = null;

            if (targetName=="waitingPoint")
            {
              BaseDefanceManager.baseDefanceManager.waitingSoldierList.Add(this);
            }


          
        }
    }



    private void Update()
    {


        CheckArrive();

        if (soldierPosition != null && agent.isActiveAndEnabled)
        {

            if (agent.remainingDistance < 1)
            {
                agent.enabled = false;
                animator.SetBool("isRun", false);
            }

        }


        if (enemies.Count > 0)
        {

            animator.SetBool("isFire", true);
            if (enemies[0] != null)
            {
                var lookPos = enemies[0].transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 4);

                if (!isActiveFire)
                {
                    for (int i = 0; i < guns.Length; i++)
                    {
                        if (guns[i] != null)
                        {
                            if (bodyPartManager.destroyedLeg ==1)
                            {
                                if(player!=null && !animator.GetBool("isRun"))
                                {
                                    StartCoroutine(guns[i].FireRoutine(enemies[0], fireSpeed));
                                    
                                    animator.SetBool("isFire", true);
                                }

                                else if (player != null && animator.GetBool("isRun"))
                                {
                                    animator.SetBool("isFire", false);
                                }

                            }

                           else if (bodyPartManager.destroyedLeg == 2)
                            {

                               
                               
                             // geri ekle   player = null;

                               StartCoroutine(guns[i].FireRoutine(enemies[0], fireSpeed));
                                

                            }

                            else
                            {
                                animator.SetBool("isFire", true);
                                StartCoroutine(guns[i].FireRoutine(enemies[0], fireSpeed));
                                

                            }



                        }

                    }

                    

                }
                

            }
            else
            {
                enemies.RemoveAt(0);
            }

           

        }
        else
        {
              animator.SetBool("isFire", false);
        }


    }

   public void LeaveFromPlayer()
    {
        if (bodyPartManager != null && bodyPartManager.destroyedLeg == 2)
        {
            transform.parent =  PlayerController.playerController.injuredSoldierParent.transform;

            robotMesh.transform.position = new Vector3(robotMesh.transform.position.x, robotMesh.transform.position.y - 2, robotMesh.transform.position.z);

            fullCollider.enabled = false;

            smallCollider.enabled = true;

            if (player != null)
            {
                player.GetComponent<PlayerSoldierManager>().soldierTransforms[id].isFull = false;

                player = null;
            }


        }
    }

    private void OnDisable()
    {
        BaseDefanceManager.baseDefanceManager.waitingSoldierCount--;
    }

    










}
