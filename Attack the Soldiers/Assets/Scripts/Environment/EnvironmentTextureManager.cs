using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class EnvironmentTextureManager : NetworkBehaviour
{
    [SerializeField] private List<Sprite> backgrounds;
    [SerializeField] private List<Sprite> environmentTextures;
    [SerializeField] private List<SpriteRenderer> environmentRenderers;
    [SerializeField] private SpriteRenderer backgroundRenderer;

    private NetworkVariable<int> backgroundIndex = new NetworkVariable<int>();
    private NetworkVariable<int> environmentIndex = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        // Suscribirse a cambios
        backgroundIndex.OnValueChanged += OnBackgroundChanged;
        environmentIndex.OnValueChanged += OnEnvironmentChanged;

        // Si soy host, elegir valores
        if (IsHost)
        {
            backgroundIndex.Value = Random.Range(0, backgrounds.Count);
            environmentIndex.Value = Random.Range(0, environmentTextures.Count);
        }

        // Aplicar valores actuales (para clientes que entran después)
        ApplyBackground(backgroundIndex.Value);
        ApplyEnvironment(environmentIndex.Value);
    }

    private void OnBackgroundChanged(int oldValue, int newValue)
    {
        ApplyBackground(newValue);
    }

    private void OnEnvironmentChanged(int oldValue, int newValue)
    {
        ApplyEnvironment(newValue);
    }

    public void ApplyRandomBackground()
    {
        ApplyBackground(Random.Range(0, backgrounds.Count));
    }

    public void ApplyRandomEnvironment()
    {
        ApplyEnvironment(Random.Range(0, environmentTextures.Count));
    }

    private void ApplyBackground(int index)
    {
        if (backgrounds.Count > index)
        {
            backgroundRenderer.sprite = backgrounds[index];
        }
    }

    private void ApplyEnvironment(int index)
    {
        if (environmentTextures.Count > index)
        {
            foreach (var renderer in environmentRenderers)
            {
                renderer.sprite = environmentTextures[index];
            }
        }
    }

    private void OnDestroy()
    {
        backgroundIndex.OnValueChanged -= OnBackgroundChanged;
        environmentIndex.OnValueChanged -= OnEnvironmentChanged;
    }
}