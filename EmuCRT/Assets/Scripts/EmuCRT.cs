using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace EmulationCRT
{
    [RequireComponent(typeof(Camera))]
    public class EmuCRT : MonoBehaviour
    {
        public const string VIDEO_DEVICE_SAVE = "VIDEO_DEVICE_SAVE";
        public const string AUDIO_DEVICE_SAVE = "AUDIO_DEVICE_SAVE";

        public Texture[] textureTest;
        private int testTexture = -1;
    
        public enum StretchModes
        {
            x43,
            x169,
            xFit
        }
    
        [Range(0, 5)] public float brightness = 1.5f;
        [Range(0, 10)] public float blur = 3.0f;
        [Range(0, 10)] public float blurSpread = 0.5f;
        [Range(0, 1)] public float blend = 1.0f;
        [Range(0, 1)] public float blackPoint = 0.0f;
        [Range(0, 1)] public float whitePoint = 1.0f;

        public Texture2D _phosphorLut;
        public LUTGenerator.Types _lutType = LUTGenerator.Types.SingleHorizontal;
    
        private Vector2Int screenResolution = Vector2Int.zero;
        private Vector2Int displayResolution = Vector2Int.zero;
        private Vector2Int inputResolution = new Vector2Int(320, 240);
        private StretchModes _stretchModes = StretchModes.x43;
        private AudioSource audioSource;
    
        WebCamTexture webcamTexture;
        private WebCamDevice[] videoWebCamDevices;
        private string videoDeviceName = "";

        private string audioDeviceName = "";
        private string[] audioDevices;
        private bool audioDisabled = false;
    
        public Material _lutMaterial;
        public Material _crtMaterial;
        public Material _blurMaterial;
        public Material _blendMaterial;

        private bool rawImage = false;
        private bool showSettings = false;
    
        void Start()
        {
            screenResolution.x = Screen.width;
            screenResolution.y = Screen.height;
            videoWebCamDevices = WebCamTexture.devices;
        
            UpdateCameraAspect();

            audioSource = GetComponent<AudioSource>();
            if(audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();

            audioSource.bypassEffects = true;
            audioSource.bypassListenerEffects = true;
            audioSource.bypassReverbZones = true;
            audioSource.priority = 255;
            audioSource.loop = true;
            audioDevices = Microphone.devices;

            if (PlayerPrefs.HasKey(VIDEO_DEVICE_SAVE))
            {
                videoDeviceName = PlayerPrefs.GetString(VIDEO_DEVICE_SAVE);
                StartVideoCapture();
            }

            if (PlayerPrefs.HasKey(AUDIO_DEVICE_SAVE))
            {
                audioDeviceName = PlayerPrefs.GetString(AUDIO_DEVICE_SAVE);
                audioDisabled = audioDeviceName == "DISABLED";
                StartAudioCapture();
            }
        }

        private void StartVideoCapture()
        {
            webcamTexture = new WebCamTexture(videoDeviceName, displayResolution.x, displayResolution.y, 30)
            {
                filterMode = FilterMode.Point, 
                anisoLevel = 0
            };
            webcamTexture.Play();
        }

        private void StartAudioCapture()
        {
            if(audioDisabled)
                return;
            audioSource.clip = Microphone.Start(audioDeviceName, true, 10, 44100);
            audioSource.Play();
        }

        private void SelectVideoDevice(int index)
        {
            videoDeviceName = videoWebCamDevices[index].name;
            PlayerPrefs.SetString(VIDEO_DEVICE_SAVE, videoDeviceName);
            StartVideoCapture();
        }

        private void SelectAudioDevice(int index)
        {
            audioDeviceName = audioDevices[index];
            PlayerPrefs.SetString(AUDIO_DEVICE_SAVE, audioDeviceName);
            StartAudioCapture();
        }

        private void DisableAudio()
        {
            audioDisabled = true;
            audioDeviceName = "DISABLED";
            audioSource.Stop();
        }

        private void StopCaptures()
        {
            webcamTexture.Stop();
            audioSource.Stop();
            videoDeviceName = "";
            audioDeviceName = "";
        }

        private void SetStretchMode(StretchModes newMode)
        {
            _stretchModes = newMode;
            UpdateCameraAspect();
        }

        public void SetLUT(LUTGenerator.Types type)
        {
            _lutType = type;
            GenerateLUT();
        }

        private void GenerateLUT()
        {
            _phosphorLut = LUTGenerator.Generate(displayResolution, _lutType);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                rawImage = !rawImage;
            }
        
            if (Input.GetKeyDown(KeyCode.S))
            {
                int currentMode = (int) _stretchModes;
                int modeCount = Enum.GetNames(typeof(StretchModes)).Length;
                int newMode = (currentMode + 1) % modeCount;
                SetStretchMode((StretchModes)newMode);
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                testTexture++;
                if (testTexture >= textureTest.Length)
                    testTexture = -1;
            }
        
            if (Input.GetKeyDown(KeyCode.L))
            {
                int currentMode = (int) _lutType;
                int modeCount = Enum.GetNames(typeof(LUTGenerator.Types)).Length;
                int newMode = (currentMode + 1) % modeCount;
                SetLUT((LUTGenerator.Types)newMode);
            }

            if (Input.GetKeyDown(KeyCode.Period))
            {
                showSettings = !showSettings;
            }
        
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                StopCaptures();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        private void UpdateCameraAspect()
        {
            Camera cam = GetComponent<Camera>();
            if (_stretchModes == StretchModes.xFit)
            {
                cam.rect = new Rect(0, 0, 1, 1);
                return;
            }

            Vector2 stretchAspect = new Vector2();
            switch (_stretchModes)
            {
                case StretchModes.x43:
                    stretchAspect.x = 4;
                    stretchAspect.y = 3;
                    break;
                case StretchModes.x169:
                    stretchAspect.x = 16;
                    stretchAspect.y = 9;
                    break;
            }
        
            float screenAspect = screenResolution.x / (float)screenResolution.y;
            float requestedAspect = stretchAspect.x / stretchAspect.y;
        
            Rect camRect = new Rect(0, 0, 1, 1);
            if (screenAspect < requestedAspect)
            {
                camRect.height =  (Screen.width / stretchAspect.x * stretchAspect.y) / Screen.height;
                camRect.y =  (1f-camRect.height) * 0.5f;
            }
            else
            {
                camRect.width =  (Screen.height / stretchAspect.y * stretchAspect.x) / Screen.width;
                camRect.x =  (1f-camRect.width) * 0.5f;
            }

            displayResolution.x = Mathf.RoundToInt(screenResolution.x * camRect.width);
            displayResolution.y = Mathf.RoundToInt(screenResolution.y * camRect.height);
            GenerateLUT();
        
            cam.rect = camRect;
        }

        private void OnGUI()
        {
            if (string.IsNullOrEmpty(videoDeviceName))
            {
                GUILayout.Box("Select Video Device");
                for (int i = 0; i < videoWebCamDevices.Length; i++)
                {
                    if (GUILayout.Button(videoWebCamDevices[i].name))
                        SelectVideoDevice(i);
                }
            }
            if (string.IsNullOrEmpty(audioDeviceName))
            {
                GUILayout.Box("Select Audio Device");
                for (int i = 0; i < audioDevices.Length; i++)
                {
                    if (GUILayout.Button(audioDevices[i]))
                        SelectAudioDevice(i);
                }

                if (GUILayout.Button("DISABLE"))
                    DisableAudio();
            }

            if (showSettings)
            {
                GUILayout.BeginVertical();
                GUILayout.Label("Settings");
            
                GUILayout.BeginVertical(GUILayout.Width(100));
                GUILayout.Label("Brightness");
                GUILayout.Label(brightness.ToString("F1"));
                brightness = GUILayout.HorizontalSlider(brightness, 0, 10);
                GUILayout.EndVertical();
            
                GUILayout.BeginVertical(GUILayout.Width(100));
                GUILayout.Label("Blur");
                GUILayout.Label(blur.ToString("F1"));
                blur = GUILayout.HorizontalSlider(blur, 0, 30);
                GUILayout.EndVertical();
            
                GUILayout.BeginVertical(GUILayout.Width(100));
            
                GUILayout.Label("Stretch Mode");
            
                if(_stretchModes == StretchModes.x43)
                    GUILayout.Box("4:3");
                else if (GUILayout.Button("4:3"))
                    SetStretchMode(StretchModes.x43);
            
                if(_stretchModes == StretchModes.x169)
                    GUILayout.Box("16:9");
                else if (GUILayout.Button("16:9"))
                    SetStretchMode(StretchModes.x169);
            
                if(_stretchModes == StretchModes.xFit)
                    GUILayout.Box("FIT");
                else if (GUILayout.Button("FIT"))
                    SetStretchMode(StretchModes.xFit);
            
                GUILayout.EndVertical();

                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"Screen Resolution {screenResolution}");
                sb.AppendLine($"Display Resolution {displayResolution}");
                sb.AppendLine($"Input Resolution {inputResolution}");
                GUILayout.Box(sb.ToString());
            
                GUILayout.EndVertical();
            }
        }

        void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            if(string.IsNullOrEmpty(videoDeviceName))
                return;

            Texture source = testTexture == -1 ? webcamTexture : textureTest[testTexture];//testTextureMario;

            if (rawImage)
            {
                Graphics.Blit(source, dest);
                return;
            }
        
            RenderTexture crtBuffer = RenderTexture.GetTemporary(displayResolution.x, displayResolution.y, 0);
            _lutMaterial.SetTexture("_LUT", _phosphorLut);
            Graphics.Blit(source, crtBuffer, _lutMaterial);
        
            RenderTexture blurBuffer = RenderTexture.GetTemporary(displayResolution.x, displayResolution.y, 0);
            _blurMaterial.SetFloat("_KernelSize", blur);
            _blurMaterial.SetFloat("_Spread", blurSpread);
            Graphics.Blit(source, blurBuffer, _blurMaterial);

            _blendMaterial.SetTexture("_BlendTex", blurBuffer);
            _blendMaterial.SetFloat("_Blend", blend);
            _blendMaterial.SetFloat("_Brighten", brightness);
            _blendMaterial.SetFloat("_BlackPoint", blackPoint);
            _blendMaterial.SetFloat("_WhitePoint", whitePoint);
        
            Graphics.Blit(crtBuffer, dest, _blendMaterial);
        
            RenderTexture.ReleaseTemporary(crtBuffer);
            RenderTexture.ReleaseTemporary(blurBuffer);
        }
    }
}

//http://filthypants.blogspot.com/2013/02/designing-large-scale-phosphor.html?m=1