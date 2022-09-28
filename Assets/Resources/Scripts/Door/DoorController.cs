using UnityEngine;

public class DoorController : MonoBehaviour {
    public Animator animator;
    private Transform player;
    private BoxCollider2D blocker;

    private bool isOpen;
    
    [SerializeField] private bool objectiveCompleted = true;

    public int distance;

    public bool inChild;
    public Animator animatorChild1;
    public Animator animatorChild2;

    private AudioSource audioSrc;
    public AudioClip audioClip;

    private void Awake()
    {
        /*blocker = GetComponent<BoxCollider2D>();
        foreach (BoxCollider2D col in GetComponentsInChildren<BoxCollider2D>()) col.enabled = false;*/
    }
    
    private void Start() {
        isOpen = false;
        
        player = GameObject.Find("Player").GetComponent<Transform>();
        
        audioSrc = GetComponent<AudioSource>();
    }
    
    private void Update()
    {
        //if (blocker != null) blocker.enabled = false;//!objectiveCompleted;
        SetCollider(!objectiveCompleted);
        
        if (Vector2.Distance(transform.position, player.position) < distance && !isOpen && objectiveCompleted) {
            if (inChild) SetChildDoors(true);
            else SetDoor(true);

            isOpen = true;
        } else if ((Vector2.Distance(transform.position, player.position) > distance || !objectiveCompleted) && isOpen) {
            if (inChild) SetChildDoors(false);
            else SetDoor(false);

            isOpen = false;
        }
    }

    private void SetCollider(bool state)
    {
        foreach (BoxCollider2D col in GetComponentsInChildren<BoxCollider2D>()) col.enabled = state;
    }

    private void SetDoor(bool state) {
        PlayDoorSound();
        animator.SetBool("isOpen", state);
    }

    private void SetChildDoors(bool state) {
        PlayDoorSound();
        if (animatorChild1.gameObject.activeSelf)
            animatorChild1.SetBool("isOpen", state);
        if (animatorChild2.gameObject.activeSelf)
            animatorChild2.SetBool("isOpen", state);
    }

    public bool ObjectiveCompleted {
        get { return objectiveCompleted; }
        set { objectiveCompleted = value; }
    }

    private void PlayDoorSound() {
        audioSrc.PlayOneShot(audioClip, 1f);
    }
}
