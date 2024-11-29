using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIStatus : MonoBehaviour
{
    private float MaxHealth = 100f;
    private float Health = 100f;

    public GameObject NPC_Corpse;

    //private GameObject healthbar;
    public Slider HP_Slider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (HP_Slider != null)
        {
            HP_Slider.value = Health;
        }
        if (Health <= 0)
        {
            
            GameObject corpse = Instantiate(NPC_Corpse, transform.position, transform.rotation);
            Destroy(corpse, 5);
            gameObject.SetActive(false);
            Invoke("Respawn", 3);
        }
        if (Health > MaxHealth) { Health = MaxHealth; }

    }

    public void Damage(float damage) { Health -= damage; HP_Slider.value = Health; print("damaged"); }

    void Respawn()
    {
        Health = MaxHealth;
        gameObject.SetActive(true);
    }
}
