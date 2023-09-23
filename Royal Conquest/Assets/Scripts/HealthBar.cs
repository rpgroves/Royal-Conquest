using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Entity entityToFollow;
    [SerializeField] Slider mySlider;
    [SerializeField] float offset = 0.5f;
    [SerializeField] GameObject barObject;

    public void CreateBar(Entity e)
    {
        entityToFollow = e;
        entityToFollow.OnDeath.AddListener(EntityDied);
    }

    void Update()
    {
        if(entityToFollow.getHealth() == entityToFollow.getMaxHealth())
            barObject.SetActive(false);
        else
            barObject.SetActive(true);
        mySlider.value = entityToFollow.getHealth() / entityToFollow.getMaxHealth();
        Vector3 position = new Vector3(entityToFollow.transform.position.x, entityToFollow.transform.position.y + offset, entityToFollow.transform.position.z);
        gameObject.transform.position = position;
    }

    void EntityDied()
    {
        Destroy(this.gameObject);
    }
}
