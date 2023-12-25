using UnityEngine.UI;
using UnityEngine;
using Settings;
using Core;

namespace UI
{
    [RequireComponent(typeof(RawImage))]
    public sealed class PetIcon : MonoBehaviour
    {
        [SerializeField] private RawImage _rawImage;

        private PetAppearance _petAppearance;
        private Transform _petContainer;
        private PetCamera _petCamera;
        private Pet _pet;

        public void Setup(Pet pet)
        {
            if (pet == null)
                return;
            
            _petContainer = GameObject.FindGameObjectWithTag("PetContainer").transform;
            _pet = pet;

            ReleaseResources();
            InitializeAppearance();
            InitializeCamera();
            InitializeRenderTexture();
        }

        // test
        private void Start()
        {
            Setup(new Pet(""));
        }
        // test

        private void InitializeRenderTexture()
        {
            if (_petCamera.Camera.targetTexture != null)
                return;
            
            _petCamera.Camera.targetTexture = new RenderTexture(new RenderTextureDescriptor(1024, 1024, RenderTextureFormat.Default));
            _rawImage.texture = _petCamera.Camera.targetTexture;
        }

        private void InitializeCamera()
        {
            _petCamera = Instantiate(SettingsProvider.Get<PrefabsSet>().PetCamera, _petContainer);
            _petCamera.SetTarget(_petAppearance.transform);
        }

        private void InitializeAppearance()
        {
            _petAppearance = Instantiate(SettingsProvider.Get<PrefabsSet>().PetAppearance, _petContainer);
        }

        private void ReleaseResources()
        {
            if (_petCamera != null)
            {
                _petCamera.Camera.targetTexture.Release();
                Destroy(_petCamera.gameObject);
            }

            if (_rawImage != null && _rawImage.texture is RenderTexture renderTexture)
            {
                renderTexture.Release();
            }

            if (_petAppearance != null)
                Destroy(_petAppearance.gameObject);
        }

        private void OnDestroy()
        {
            ReleaseResources();
        }
    }
}