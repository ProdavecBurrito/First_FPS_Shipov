
using UnityEngine;

public class MiniMapCont : MonoBehaviour
{
    [SerializeField]Transform player;
    Camera mapCam;
    Vector3 newPos;
    [SerializeField] Shader repShader;

    private void Start()
    {
        mapCam = GetComponent<Camera>();
        player = FindObjectOfType<SinglePlayer>().transform;
        repShader = Shader.Find("Toon/Basic Outline");

        if (repShader)
        {
            mapCam.SetReplacementShader(repShader, "RenderType"); 
        }
    }

    private void OnDisable()
    {
        mapCam.ResetReplacementShader();
    }

    private void LateUpdate()
    {
        if(player)
        {
            newPos = player.position;
            newPos.y = transform.position.y;
            transform.position = newPos;

            transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
        }
    }
}
