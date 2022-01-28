using TMPro;
using UnityEngine;

namespace Menu
{
    public class TitleScreenManager : MonoBehaviour
    {
       [SerializeField] private TMP_Text title;
       [SerializeField] private TMP_Text description;


       public void Setup(TitleScreenData data)
       {
           title.text = data.name;
           description.text = data.description;
           
       }
       

    }
}