using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour   // Добавить анимацию DamageTaken и ее выключение
    {
        private Rigidbody2D rb;
        private Animator anim; // Интерфейс для контроля анимационной системы Mecanim.
        private Vector2 v2;
        public Main main;
        public Transform groundCheck;

        bool isGrounded;

        private float speed;
        private float jumpForce = 6f;
        public float moveSpeed;

        int curHp;    
        int maxHp = 1;



        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            curHp = maxHp;

        if (PlayerPrefs.HasKey("xPos"))
            transform.position = new Vector2(PlayerPrefs.GetFloat("xPos"), PlayerPrefs.GetFloat("yPos"));
       
       
        }


        private void FixedUpdate()
        {

            if (((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)   //Прыжок + анимация прыжка
            {
                anim.SetBool("IsJumping", true);
                rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            }

            else
            {
                anim.SetBool("IsJumping", false);                 // Анимация прыжка выкл
            }

           // Move();

        }

        private void Update()
        {
            speed = Input.GetAxis("Horizontal");

        anim.SetFloat("SpeedA", Mathf.Abs(speed));          //Прописали, анимацию которая будет зависеть от числа SpeedA ( оно у нас больше 0.1 или меньше )

            CheckGround();
            Flip();

            if (transform.position.y < -4)
            {
                Invoke("Lose", 2f);

            }
        }


        //  private void Move()    // движение персонажа
        //  {
        //      v2.Set(speed * moveSpeed, rb.velocity.y);
        //      rb.velocity = v2;
        //  }

        void CheckGround()   // проверка на землю
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.1f);
            isGrounded = colliders.Length > 1;
        }



        private void Flip()   // поворот игрока влево и вправо
        {
            if (Input.GetAxis("Horizontal") < 0)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }

            if (Input.GetAxis("Horizontal") > 0)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }

        public void RecountHp(int deltaHP)   // Изменение хп + смерть
        {
            curHp = curHp + deltaHP;

            if (curHp <= 0)
            {
            anim.SetBool("DamageTaken", true);
            GetComponent<Collider2D>().enabled = false;
            Invoke("Lose", 2f);                                  // обращаемся к Lose
            }
        }



        void Lose()     //Получаем метод "Lose" из скрипта "Main"
        {
            main.GetComponent<Main>().Lose();

        }

        private void OnTriggerEnter2D(Collider2D collision)  //создаем собираемый ресурс - гемы
        {
        if (collision.gameObject.tag == "CheckPoint")
        {
            PlayerPrefs.SetFloat("xPos", transform.position.x);
            PlayerPrefs.SetFloat("yPos", transform.position.y);
            Destroy(collision.gameObject);
        }
      
        }
    }

