using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    void Start()
    {
        /*Los s�mbolos mayor que y menor que. Estos han sido a�adidos porque el 
         *m�todo GetComponent es gen�rico. Un m�todo gen�rico es aquel que tiene 
         *dos configuraciones diferentes de par�metros: par�metros normales y 
         *par�metros de tipo. Los par�metros enlistados entre los s�mbolos de 
         *mayor y menor que son los par�metros de tipo.
         */
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();

    }

    /*Para asegurarte de que el vector de movimiento y rotaci�n est� configurado 
     * para que funcione en conjunto con OnAnimatorMove, cambia tu m�todo Update 
     * a uno que sea un m�todo FixedUpdate*/

    /*Este es otro m�todo especial que Unity invoca autom�ticamente, pero este 
     *funciona en conjunto con la f�sica. En vez de invocarse en cada marco, FixedUpdate 
     *se invoca antes de que el sistema de f�sica resuelva cualquier colisi�n 
     *y otras interacciones que hayan ocurrido. Por defecto se invoca 50 veces por segundo.*/

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        //Comprueba si la entrada del eje horizontal es similar a cero y retorna true o false
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        /* El primer par�metro es el nombre del par�metro Animator al que quieres 
         * asignarle un valor y el segundo es el valor que les quieres asignar.
         * */
        m_Animator.SetBool("IsWalking", isWalking);

        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop();
        }

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);
    }

    private void OnAnimatorMove()
    {
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        m_Rigidbody.MoveRotation(m_Rotation);
    }
}
