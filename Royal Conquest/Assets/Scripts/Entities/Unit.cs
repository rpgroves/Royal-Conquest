using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Entity
{
    [SerializeField] bool isMovingNorth = true;
    [SerializeField] bool isNodeFollower = true;
    [SerializeField] float updateDistance = 5.0f;
    [SerializeField] float attackDistance = 5.0f;
    [SerializeField] float attackDistanceBuildings = 5.0f;
    [SerializeField] WeaponParent weaponParent;
    GameObject currentTarget = null;
    List<GameObject> nearbyTargets;

    void Start()
    {
        nearbyTargets = new List<GameObject>();
        HealthBar healthBar = Instantiate(healthBarPrefab, GameObject.Find("Canvas").transform).GetComponent<HealthBar>();
        healthBar.CreateBar(this);
    }

    void Update()
    {
        if(!isEntityInControl)
            return;
        HandleTarget();
        HandleMovement();
        HandleHealing();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Unit" || other.tag == "Building" || other.tag == "Player")
        {
            //Debug.Log("an entity trigger " + other.gameObject.name);
            if(other.gameObject.GetComponent<Entity>().getTeam() != this.getTeam())
            {
                nearbyTargets.Add(other.gameObject);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(nearbyTargets.Contains(other.gameObject))
        {
            nearbyTargets.Remove(other.gameObject);
            if(other.gameObject == currentTarget)
                currentTarget = null;
        }
    }

    void HandleTarget()
    {
        GameObject newTarget = null;

        for(int i = 0; i < nearbyTargets.Count; i++)
        {
            if(nearbyTargets[i] == null)
            {
                nearbyTargets.RemoveAt(i);
            }
        }

        //are we already targeting an enemy / building and we are in range
        if(currentTarget != null && currentTarget.tag != "Node")
        {
            //are we close enough to that enemy to attack?
            if(((TwoObjectDistance(currentTarget, gameObject) < attackDistance) && (currentTarget.tag == "Unit" || currentTarget.tag == "Player")) || ((TwoObjectDistance(currentTarget, gameObject) < attackDistanceBuildings) && currentTarget.tag == "Building"))
            {
                //Debug.Log("ATTACK");
                ChangeMoveVector(new Vector2(0.0f, 0.0f));
                if(weaponParent != null)
                {
                    weaponParent.setTargetDirection(-(currentTarget.transform.position - gameObject.transform.position).normalized);
                    weaponParent.Attack();
                }
                return;
            }
            else // is there a closer enemy in range? (avoid chasing one enemy if you cant currently hit them)
            {
                foreach(GameObject gb in nearbyTargets)
                {
                    if(TwoObjectDistance(gameObject, gb) < TwoObjectDistance(gameObject, currentTarget))
                    {
                        newTarget = gb;
                    }
                }
            }
        }

        //is there a nearby building or enemy in range
        else if(nearbyTargets.Count != 0)
        {
            //Debug.Log("theres an enemy nearby i should turn to!");
            foreach(GameObject gb in nearbyTargets)
            {
                if(newTarget == null)
                {
                    newTarget = gb;
                    continue;
                }
                if(TwoObjectDistance(gameObject, gb) < TwoObjectDistance(gameObject, newTarget))
                {
                    newTarget = gb;
                }
            }
        }

        //are we already going to a node?
        else if(isNodeFollower && currentTarget != null && currentTarget.tag == "Node")
        {
            //are we too close to that node?
            if(TwoObjectDistance(currentTarget, gameObject) < updateDistance)
            {
                Node node = currentTarget.GetComponent<Node>();
                //Debug.Log("Finding the next node");
                if(isMovingNorth && node.isNorthEnd() == false)
                    newTarget = node.getNextNorth().gameObject;
                else if(!isMovingNorth && node.isSouthEnd() == false)
                    newTarget = node.getNextSouth().gameObject;
            }
        }

        //nothing nearby and no current target, we need to find a node nearby
        else if(isNodeFollower && nearbyTargets.Count == 0)
        {
            //Debug.Log("Finding nearest node");
            Node[] nodes = FindObjectsOfType<Node>();
            foreach(Node n in nodes)
            {
                //Debug.Log("Checking Node " + n.gameObject.name);
                if(newTarget == null)
                {
                    newTarget = n.gameObject;
                    continue;
                }
                if(TwoObjectDistance(gameObject, n.gameObject) < TwoObjectDistance(gameObject, newTarget))
                {
                    newTarget = n.gameObject;
                }
            }
        }

        //if we have a new target we need to set it
        if(newTarget != null)
        {
            currentTarget = newTarget;
            //Debug.Log("new target " + currentTarget.gameObject.name);
        }
        if(currentTarget != null)
        {
            weaponParent.setTargetObject(currentTarget);
            ChangeMoveVector((currentTarget.transform.position - gameObject.transform.position).normalized);
            if(weaponParent != null)
                weaponParent.setTargetDirection(-(currentTarget.transform.position - gameObject.transform.position).normalized);
        }
        else
            weaponParent.setTargetObject(null);
    }

    float TwoObjectDistance(GameObject ob1, GameObject ob2)
    {
        if(ob1 == null || ob2 == null)
            return 0.0f;
        return Vector3.Distance(ob1.transform.position, ob2.transform.position);
    }
}
