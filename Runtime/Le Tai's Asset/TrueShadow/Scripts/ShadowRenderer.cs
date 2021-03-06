using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace LeTai.TrueShadow
{
[AddComponentMenu("")]
[ExecuteAlways]
public partial class ShadowRenderer : MonoBehaviour, ILayoutIgnorer, IMaterialModifier, IMeshModifier
{
    public bool ignoreLayout => true;

    internal CanvasRenderer CanvasRenderer { get; private set; }

    TrueShadow    shadow;
    RectTransform rt;
    Graphic       graphic;

    public static void Initialize(TrueShadow shadow, ref ShadowRenderer renderer)
    {
        if (renderer && renderer.shadow == shadow)
        {
            renderer.gameObject.SetActive(true);
            return;
        }

        var obj = new GameObject($"{shadow.gameObject.name}'s Shadow") {
#if LETAI_TRUESHADOW_DEBUG
            hideFlags = DebugSettings.Instance.showObjects
                            ? HideFlags.DontSave
                            : HideFlags.HideAndDontSave
#else
            hideFlags = HideFlags.HideAndDontSave
#endif
        };

        shadow.SetHierachyDirty();

        var rt = obj.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.zero;

        Graphic graphic;
        var     type = shadow.Baked ? GraphicType.Image : GraphicType.RawImage;
        switch (type)
        {
            case GraphicType.Image:
                var image = obj.AddComponent<Image>();
                image.useSpriteMesh = true;

                graphic = image;
                break;
            case GraphicType.RawImage:
                graphic = obj.AddComponent<RawImage>();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        graphic.raycastTarget = false;
        graphic.color         = shadow.Color;

        renderer         = obj.AddComponent<ShadowRenderer>();
        renderer.shadow  = shadow;
        renderer.rt      = rt;
        renderer.graphic = graphic;

        // renderer.RecreateGraphic(shadow.Baked ? GraphicType.Image : GraphicType.RawImage);

        renderer.UpdateMaterial();

        renderer.CanvasRenderer = obj.GetComponent<CanvasRenderer>();
        renderer.CanvasRenderer.SetColor(shadow.CanvasRenderer.GetColor());
        renderer.CanvasRenderer.SetAlpha(shadow.CanvasRenderer.GetAlpha());

        renderer.ReLayout();
    }

    public void UpdateMaterial()
    {
        var mat = shadow.BlendMode.GetMaterial();
        graphic.material = mat ? mat : shadow.GetShadowRenderingNormalMaterial();
    }

    enum GraphicType
    {
        Image,
        RawImage
    }

//     void RecreateGraphic(GraphicType type)
//     {
//         var graphics = GetComponents<Graphic>();
//         for (var i = 0; i < graphics.Length; i++)
//         {
// #if UNITY_EDITOR
//             DestroyImmediate(graphics[i]);
// #else
//             Destroy(graphics[i]);
// #endif
//     }

    public void ReLayout()
    {
        var hostRT = shadow.RectTransform;
        if (!hostRT)
        {
            CanvasRenderer.SetAlpha(0);
            return;
        }

        var casterSize = hostRT.rect.size;
        if (casterSize == Vector2.zero)
        {
            CanvasRenderer.SetAlpha(0);
            return;
        }

        var padding = Mathf.CeilToInt(shadow.Size);
        var tw      = Mathf.CeilToInt(casterSize.x + shadow.Size * 2);
        var th      = Mathf.CeilToInt(casterSize.y + shadow.Size * 2);

        var expansion = padding * Vector2.one;
        var size      = new Vector2(tw, th);
        rt.sizeDelta = size;

        // pivot should be relative to the un-blurred part of the texture, not the whole mesh
        rt.pivot = hostRT.pivot * casterSize / size + expansion / size;

        var offset = shadow.Offset.WithZ(0);

        if (!shadow.ShadowAsSibling)
        {
            offset = hostRT.InverseTransformDirection(offset);
        }

        rt.localPosition = shadow.ShadowAsSibling ? hostRT.localPosition + offset : offset;
        rt.localRotation = shadow.ShadowAsSibling ? hostRT.localRotation : Quaternion.identity;
        rt.localScale    = shadow.ShadowAsSibling ? hostRT.localScale : Vector3.one;

        graphic.color = shadow.Color;
        CanvasRenderer.SetColor(shadow.CanvasRenderer.GetColor());
        CanvasRenderer.SetAlpha(shadow.CanvasRenderer.GetAlpha());
    }

    public void SetTexture(Texture texture)
    {
        if (!(graphic is RawImage))
        {
            // RecreateGraphic(GraphicType.RawImage);
        }

        CanvasRenderer.SetTexture(texture);
        ((RawImage) graphic).texture = texture;
    }

    public void SetSprite(Sprite sprite)
    {
        if (!(graphic is Image))
        {
            // RecreateGraphic(GraphicType.Image);
        }

        ((Image) graphic).sprite = sprite;
    }

    public void ModifyMesh(VertexHelper vertexHelper)
    {
        if (!shadow)
            return;

        shadow.ModifyShadowRendererMesh(vertexHelper);
    }

    public void ModifyMesh(Mesh mesh)
    {
        Debug.Assert(true, "This should only be called on old unsupported Unity version");
    }

    protected virtual void LateUpdate()
    {
        if (!shadow)
            Dispose();
    }

    bool willBeDestroyed;

    protected virtual void OnDestroy()
    {
        willBeDestroyed = true;
    }

    public void Dispose()
    {
        if (willBeDestroyed) return;

        if (shadow && shadow.ShadowAsSibling)
        {
            // Destroy does not happen immediately. Want out of hierarchy.
            gameObject.SetActive(false);
            transform.SetParent(null);
        }

#if UNITY_EDITOR
        // This look redundant but is necessary!
        if (!Application.isPlaying && !UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            DestroyImmediate(gameObject);
        else if (Application.isPlaying)
            Destroy(gameObject);
#else
            Destroy(gameObject);
#endif
    }
}
}
