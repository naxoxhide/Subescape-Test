using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody;

    private Animator Animaciones;

    [SerializeField]private float velocidadMovimiento;

    private bool ataque;

    private bool mirarDerecha;

    [SerializeField]private Transform[] groundPoints;

    [SerializeField]private float groundRadius;

    [SerializeField]private LayerMask whatIsGround;

    private bool EnSuelo;

	// Use this for initialization
	void Start ()
    {
        mirarDerecha = true;
        m_Rigidbody = GetComponent<Rigidbody2D>();
        Animaciones = GetComponent<Animator>();

	}

    void Update()
    {
        HandleInput();
    }

	
	// Update is called once per frame
	void FixedUpdate ()
    {
        float horizontal = Input.GetAxis("Horizontal");

        EnSuelo = enSuelo();

        handleMovement(horizontal);

        darVuelta(horizontal);

        handleAttack();

        reiniciarValues();
	}

    private void handleMovement(float horizontal)
    {
        if (!this.Animaciones.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            m_Rigidbody.velocity = new Vector2(horizontal * velocidadMovimiento, m_Rigidbody.velocity.y);
        }
        
        Animaciones.SetFloat("Speed", Mathf.Abs(horizontal));
    }

    private void handleAttack()
    {
        if (ataque && !this.Animaciones.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            Animaciones.SetTrigger("Attack");
            m_Rigidbody.velocity = Vector2.zero;
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ataque = true; 
        }
    }

    private void darVuelta(float horizontal)
    {
        if (horizontal > 0 && !mirarDerecha || horizontal < 0 && mirarDerecha)
        {
            mirarDerecha = !mirarDerecha;

            Vector2 theScale = transform.localScale;

            theScale.x *= -1;

            transform.localScale = theScale;
        }
    }

    private void reiniciarValues()
    {
        ataque = false;  
    }

    private bool enSuelo()
    {
        if(m_Rigidbody.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}

