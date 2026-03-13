using UnityEngine;

public class PartItem2 : MonoBehaviour
{


  

    [SerializeField] private float detectionRange = 5.0f;
    private Transform playerTransform;

    private Renderer targetRenderer;
    private Material targetMaterial;
    [ColorUsage(true, true)]
    [SerializeField] private Color glowColor = Color.cyan;
    [SerializeField] private float glowIntensity = 2.0f;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerTransform = player.transform;
        }

       

        targetRenderer = GetComponent<Renderer>();
        if (targetRenderer != null)
        {
            targetMaterial = targetRenderer.material;

            SetEmission(Color.black, 0);
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);


        if (distance < detectionRange)
        {
            Color finalColor = glowColor * Mathf.LinearToGammaSpace(glowIntensity);

            targetMaterial.SetColor("_EmissionColor", finalColor);


            targetMaterial.EnableKeyword("_EMISSION");

        }
        else
        {
            targetMaterial.SetColor("_EmissionColor", Color.black);
            targetMaterial.DisableKeyword("_EMISSION");
        }
    }


    void SetEmission(Color color,float intensity)
    {
        if (targetMaterial == null) return;
        Color finalColor = color * Mathf.LinearToGammaSpace(intensity);

        targetMaterial.SetColor("_EmissionColor", finalColor);


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            PlayerInventory inventory = other.GetComponent<PlayerInventory>();

            if (inventory != null)
            {
                inventory.AddPart2(1);
                OnDestroy();
                Destroy(gameObject);
                
            }
        }


        
    }

    private void OnDestroy()
    {

        if (targetMaterial != null)
        {
            Destroy(targetMaterial);
        }
        
    }
}
