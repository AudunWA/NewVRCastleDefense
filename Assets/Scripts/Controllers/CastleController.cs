using UnityEngine;
using UnityEngine.UI;

public class CastleController : MonoBehaviour {
    public Castle Castle { get; set; }
    private const string evilCastleTag = "EvilCastle";
    private const string goodCastleTag = "GoodCastle";
    double maxHealth;
    public Image HealthBar;

    // Use this for initialization
    void Start () {
        GameObject goWorld = GameObject.FindWithTag("World");
        WorldController worldController = goWorld.GetComponent<WorldController>();

        if (gameObject.tag.Equals(evilCastleTag))
        {
            worldController.GoEvilCastle = gameObject;
            if(Castle == null)
            {
                Castle = worldController.EvilPlayer.Castle;
            }
        }else
        {
            worldController.GoGoodCastle = gameObject;
            if (Castle == null)
            {
                Castle = worldController.GoodPlayer.Castle;
            }
        }
        maxHealth = Castle.Health;
     
	}
	
	// Update is called once per frame
	void Update () {
        if (HealthBar != null)
        {
            HealthBar.fillAmount = (float)(Castle.Health / maxHealth);
        }
        if (Castle.Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
