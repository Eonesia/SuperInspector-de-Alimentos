using UnityEngine;

public class FootstepPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] walkClips;
    public AudioClip[] runClips;
    public float walkStepRate = 0.5f;
    public float runStepRate = 0.3f;

    public float minVelocity = 0.1f;

    private CharacterController controller;
    private float stepTimer;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        bool isMoving = controller.isGrounded && controller.velocity.magnitude > minVelocity;
        bool isRunning = Input.GetKey(KeyCode.LeftShift); // Puedes cambiar esto según tu sistema

        if (isMoving)
        {
            stepTimer += Time.deltaTime;
            float currentRate = isRunning ? runStepRate : walkStepRate;

            if (stepTimer >= currentRate)
            {
                PlayFootstep(isRunning);
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    void PlayFootstep(bool running)
    {
        AudioClip[] clips = running ? runClips : walkClips;
        if (clips.Length == 0) return;

        int index = Random.Range(0, clips.Length);
        audioSource.PlayOneShot(clips[index]);
    }
}

