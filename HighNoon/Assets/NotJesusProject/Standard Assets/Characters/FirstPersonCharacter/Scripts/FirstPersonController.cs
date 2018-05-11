using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (CharacterController))]
    public class FirstPersonController : MonoBehaviour //CUSTOM FOR HIGH NOON
    {
        [SerializeField] public bool m_IsWalking; //Are we walking
        [SerializeField] public float m_WalkSpeed; //How fast we walk
        [SerializeField] public float m_RunSpeed; //How fast we run
        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten; //Length in between step
        [SerializeField] private float m_JumpSpeed; //How much to jump by
        [SerializeField] private float m_StickToGroundForce; //
        [SerializeField] private float m_GravityMultiplier; //Self-explanatory
        [SerializeField] private MouseLook m_MouseLook; //For looking around
        [SerializeField] private bool m_UseFovKick; //Whether or not kick FOV
        [SerializeField] private FOVKick m_FovKick = new FOVKick(); //The fOV kick itself
        [SerializeField] private bool m_UseHeadBob; //Whether or not to use headbobbing
        [SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob(); //Bobbing for walking/running
        [SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob(); //Bobbing for jumping
        [SerializeField] private float m_StepInterval; //Interval per step

        private Camera m_Camera;
        private bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;
        public AudioClip tempMusic;
        public float speed;
        public AudioClip tempKill;
        public AudioClip tempAnnouncerKillStreak;
        //HIGH NOON VARS
        private bool isCrouched = false;
        private bool isRolling = false;
        public float distThreshold = .5f;

        //HIGH NOON VARS BUT FROM THE RPG CON
        private Vector3 targetDashDirection;
        protected Animator animator;
        public float rollduration;
        public Rigidbody rb;

        private void Start() //Initialization
        {
            m_CharacterController = GetComponent<CharacterController>(); 
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_FovKick.Setup(m_Camera);
            m_HeadBob.Setup(m_Camera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle/2f;
            m_Jumping = false;
			m_MouseLook.Init(transform , m_Camera.transform);
            SoundManager.instance.Play(tempMusic, "mx");



            //HIGH NOON
            animator = GetComponentInChildren<Animator>();
        }
        
        private void Update()
        {
            //RotateView();
         
            if (!m_Jump) // the jump state needs to read here to make sure it is not missed || If we're not jumping, then see if there's input
            {
                if(this.gameObject.name == "Player1")
                {
                    m_Jump = CrossPlatformInputManager.GetButtonDown("Abutton");
                }
                if (this.gameObject.name == "Player2")
                {
                    m_Jump = CrossPlatformInputManager.GetButtonDown("aJump");
                }
            }

            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded) //If we were in the air and now we're on the ground, do the jump bob, don't apply up force, 
                //and recognize that were not jumping now
            {
                StartCoroutine(m_JumpBob.DoBobCycle());
                m_MoveDir.y = 0f;
                m_Jumping = false;
            }

            if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded) //If we're not grounded, and we're not jumping and we were grounded, don't apply jump force
            {
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;

            if (m_CharacterController.isGrounded == true) //CHECKING FOR ROLLING IF YOU'RE ON THE GROUND
            {
                CheckForRolling();
            }

            #region HIGH NOON MOVEMENT ANIM CALLS

            float velocityXel = transform.InverseTransformDirection(rb.velocity).x;
            float velocityZel = transform.InverseTransformDirection(rb.velocity).z;

            if (!m_IsWalking) //FOR RUNNING
            {
                animator.SetFloat("Velocity X", 1);
                animator.SetFloat("Velocity Z", 1);
                animator.SetBool("Moving", true); ;
            }

            else if (m_IsWalking)
            {
                animator.SetFloat("Velocity X", .5f);
                animator.SetFloat("Velocity Z", .5f);
                animator.SetBool("Moving", true); ;
            }

            if (Input.GetButtonDown("Abutton") && m_CharacterController.isGrounded) //FOR JUMPING
            {
                animator.SetInteger("Jumping", 1);
                animator.SetTrigger("JumpTrigger");
                animator.SetInteger("Jumping", 2);
            }

            RaycastHit hit;
            Vector3 offset = new Vector3(0, -0.5f, 0);
            if (Physics.Raycast((transform.position + offset), -Vector3.up, out hit, 100f))
            {
                Debug.DrawRay(transform.position + offset, -Vector3.up, Color.magenta);
                if (hit.distance < distThreshold)
                {
                    animator.SetInteger("Jumping", 0);
                }
            }

            if (Input.GetAxis("Left Stick Vertical") == -1) //For
            {
                animator.SetFloat("Velocity X", 1);
                animator.SetBool("Moving", true);
            }
            else if (Input.GetAxis("Left Stick Vertical") < 0 && Input.GetAxis("Left Stick Vertical") > -1) //For
            {
                animator.SetFloat("Velocity X", .5f);
                animator.SetBool("Moving", true);
            }

            if (Input.GetAxis("Left Stick Horizontal") > 0) //Right
            {
                //animator.SetTrigger("StrafeRight");
            }

            if (Input.GetAxis("Left Stick Vertical") > 0) //Back
            {
                //animator.SetTrigger("StrafeBack");
            }

            if (Input.GetAxis("Left Stick Horizontal") < 0) //Left
            {
                //animator.SetTrigger("StrafeLeft");
            }

            if (Input.GetAxis("Left Stick Horizontal") == 0 && Input.GetAxis("Left Stick Vertical") == 0)
            {
                animator.SetFloat("Velocity X", 0);
                animator.SetBool("Moving", false);
            }
            #endregion
        }

        public void FixedUpdate()
        {
            /*float speed;*/
            if (this.gameObject.name == "Player1")
            {
                GetInput1(out speed);
            }
            if (this.gameObject.name == "Player2")
            {
                GetInput2(out speed);
            }
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward*m_Input.y + transform.right*m_Input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                               m_CharacterController.height/2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x*speed;
            m_MoveDir.z = desiredMove.z*speed;


            if (m_CharacterController.isGrounded) //If we're on the ground, apply the stick to ground force.
            {
                m_MoveDir.y = -m_StickToGroundForce;

                if (m_Jump) //If we give input to jump, make us jump
                {
                    m_MoveDir.y = m_JumpSpeed;
                    m_Jump = false;
                    m_Jumping = true;
                }
            }
            else
            {
                m_MoveDir += Physics.gravity*m_GravityMultiplier*Time.fixedDeltaTime; //Otherwise, do gravity stuff
            }
            m_CollisionFlags = m_CharacterController.Move(m_MoveDir*Time.fixedDeltaTime); //This is where we call to move

            ProgressStepCycle(speed); //Do the step cycle
            UpdateCameraPosition(speed); //Update the camera

            m_MouseLook.UpdateCursorLock(); //Cursor locking stuff
        }

        /*private void RotateView()
        {
            m_MouseLook.LookRotation(transform, m_Camera.transform);
        }*/

        private void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0)) //If we are moving, then set the step cycle to what's below
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed*(m_IsWalking ? 1f : m_RunstepLenghten)))*
                             Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;
            
        }
        
        private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                return;
            }
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                      (speed*(m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }
           
            if (Input.GetButton("B") == true && isCrouched == false)  //CROUCHING
            {
                newCameraPosition.y = (m_Camera.transform.localPosition.y / 2);
            }
            m_Camera.transform.localPosition = newCameraPosition;
        } //CROUCHING IS IN HERE
        
        public void GetInput1(out float speed)
        {

            // Read input
            float horizontal = CrossPlatformInputManager.GetAxis("Left Stick Horizontal");
            float vertical = -CrossPlatformInputManager.GetAxis("Left Stick Vertical");

            bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            m_IsWalking = !Input.GetButton("Left Stick Button"); //this one actually does the sprint(hold down L-Stick)
#endif
            // set the desired speed to be walking or running
            speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }
            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }
        }
        public void GetInput2(out float speed)
        {
            // Read input
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxis("Vertical");

            bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            m_IsWalking = !Input.GetButton("Sprint"); //this one actually does the sprint(hold down L-Stick)
#endif
            // set the desired speed to be walking or running
            speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }
        }
        
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
        }

        #region ROLLING FUNCTIONS

        public void CheckForRolling() //FOR HIGH NOON
        {
            if (Input.GetButtonDown("Left Bumper") == true && !isRolling && m_CharacterController.isGrounded == true)
            {
                if (Input.GetAxis("Left Stick Vertical") != 0 || Input.GetAxis("Left Stick Horizontal") != 0)
                {
                    StartCoroutine(_DirectionalRoll(Input.GetAxis("Left Stick Vertical"), Input.GetAxis("Left Stick Horizontal")));
                }
            }
        }

        public IEnumerator _DirectionalRoll(float x, float v)
        {
            if (Input.GetAxis("Left Stick Vertical") == -1)
            {
                StartCoroutine(_Roll(1));
            }
            if (Input.GetAxis("Left Stick Horizontal") == 1)
            {
                StartCoroutine(_Roll(2));
            }
            if (Input.GetAxis("Left Stick Vertical") == 1)
            {
                StartCoroutine(_Roll(3));
            }
            if (Input.GetAxis("Left Stick Horizontal") == -1)
            {
                StartCoroutine(_Roll(4));
            }
            yield return null;
        }

        public IEnumerator _Roll(int rollNumber)
        {
            animator.SetInteger("Action", rollNumber);
            animator.SetTrigger("RollTrigger");

            isRolling = true;
            yield return new WaitForSeconds(rollduration);
            isRolling = false;
        }
        #endregion
    }
}