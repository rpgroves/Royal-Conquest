using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Entity entityToFollow;
    [SerializeField] Slider mySlider;
    [SerializeField] float offset = 0.5f;

    public void CreateBar(Entity e)
    {
        entityToFollow = e;
    }

    void Update()
    {
        mySlider.value = entityToFollow.getHealth() / entityToFollow.getMaxHealth();
        Vector3 position = new Vector3(entityToFollow.transform.position.x, entityToFollow.transform.position.y + offset, entityToFollow.transform.position.z);
        if(entityToFollow != null)
            gameObject.transform.position = position;
    }
}
