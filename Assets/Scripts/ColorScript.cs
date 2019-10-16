using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColorScript : MonoBehaviour {

    public bool isRandom = true;

    public Action<Color> OnColorChanged;

    public enum ColorNames { White, Red, Blue, Green };

    private ColorNames colorName;
    public ColorNames ColorName
    {
        get
        {
            return colorName;
        }
        set
        {
            colorName = value;
            color = colorDictionary[colorName];

            OnColorChanged?.Invoke(color);

            var meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.material.color = color;
                return;
            }

            var spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = color;
                return;
            }
        }
    }

    public Color color;

    static Dictionary<ColorNames, Color> colorDictionary = new Dictionary<ColorNames, Color>()
    {
        { ColorNames.Blue, Color.blue },
        { ColorNames.Red, Color.red },
        { ColorNames.Green, Color.green },
        { ColorNames.White, Color.white }
    };

    public void SetRandom()
    {
        //ColorName = Util.RandomEnumValue<ColorNames>();

        var colorList = Enum.GetValues(typeof(ColorNames)).Cast<ColorNames>().ToList();
        ColorName = Util.randomProc(20) ?
            ColorNames.White :
            colorList[UnityEngine.Random.Range(0, colorList.Count)];

        //System.Random random = new System.Random();
        //ColorName = (ColorNames)values.GetValue(random.Next(values.Length));
    }

    private void Start()
    {
        // Test
        if (isRandom)
            SetRandom();
        //Debug.Log(colorName);
    }

}
