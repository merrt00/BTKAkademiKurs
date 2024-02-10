using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public float health;
    bool dead = false;
    public Transform bullet , floatingText, bloodParticle;
    Transform muzzle;
    public float bulletSpeed;
    public GameObject tryAgainScreen;

    public Slider slider;
    bool mouseIsNotOverUI;
    // Start is called before the first frame update
    void Start()
    {
        muzzle = transform.GetChild(1);
        slider.maxValue = health;
        slider.value = health;
    }

    // Update is called once per frame
    void Update()
    {
        mouseIsNotOverUI = EventSystem.current.currentSelectedGameObject == null;
        if (Input.GetMouseButtonDown(0) && mouseIsNotOverUI )
        {
            ShootBullet();
        }
    }

    public void GetDamage(float damage)
    {
        Instantiate(floatingText, transform.position, Quaternion.identity).GetComponent<TextMesh>().text = damage.ToString();
        if ((health - damage) >= 0)
        {
            health -= damage;
        }
        else
        {
            health = 0;
        }
        slider.value = health;
        AmIDead();
    }

    void AmIDead()
    {
        if (health <= 0)
        {
            Destroy(Instantiate(bloodParticle, transform.position, Quaternion.identity),3f);
            DataManager.Instance.LoseProcess();
            dead = true;
            Destroy(gameObject);
            tryAgainScreen.SetActive(true);

        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Water")
        {

            Destroy(Instantiate(bloodParticle, transform.position, Quaternion.identity), 3f);
            DataManager.Instance.LoseProcess();
            dead = true;
            Destroy(gameObject);
            tryAgainScreen.SetActive(true);
        }
        if(other.tag  == "End")
        {
            SceneManager.LoadScene(2);
        }
        if (other.tag == "End1")
        {
            SceneManager.LoadScene(3);
        }
    }

    void ShootBullet()
    {
        Transform tempBullet;
        tempBullet = Instantiate(bullet , muzzle.position, Quaternion.identity);
        tempBullet.GetComponent<Rigidbody2D>().AddForce(muzzle.forward * bulletSpeed);
        DataManager.Instance.ShotBullet++;
    }
}
