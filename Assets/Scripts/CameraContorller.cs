using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CameraContorller : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private AudioClip[] clips;
    private AudioSource audioSource;
    private PlayerController playerController;

    private void Awake()
    {
        playerController = player.GetComponent<PlayerController>();
        audioSource = gameObject.GetComponent<AudioSource>();
        StartCoroutine(CheckTime());
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform);

        if (player.transform.position.x - transform.position.x > 10f)
        {
            transform.position = new Vector3(player.transform.position.x - 10f, transform.position.y, transform.position.z);
        }
        if (player.transform.position.x - transform.position.x < 5f)
        {
            transform.position = new Vector3(player.transform.position.x - 5f, transform.position.y, transform.position.z);
        }
        if (player.transform.position.z - transform.position.z < -6)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z + 6);
        }
        if (player.transform.position.z - transform.position.z > 6)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z - 6);
        }
        transform.position = new Vector3(transform.position.x, player.transform.position.y + 28f, transform.position.z);

        float getVertical = Input.GetAxis("Vertical");
        if (getVertical != 0)
        {
            float zRange = (player.transform.position.z - transform.position.z) / 100 * Mathf.Abs(getVertical);
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + zRange);
        }

    }

    IEnumerator CheckTime()
    {
        if (playerController.GetTimeWithoutLight() > 0.5f && !audioSource.isPlaying)
        {
            PlayLoseAudio();
        } 
        else if(playerController.GetTimeWithoutLight() < 0.5f && audioSource.isPlaying)
        {
            audioSource.volume = 0.2f;
        }
        if (playerController.GetTimeWithoutLight() > 3f)
        {
            SceneManager.LoadScene(0);
        }

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(CheckTime());
    }

    private void PlayLoseAudio()
    {
        audioSource.volume = 1f;
        audioSource.PlayOneShot(clips[0]);
    }
}
