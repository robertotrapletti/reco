using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Content;
using Android.Provider;
using Android.Graphics;
using RecoLibrary;

namespace Demo
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        ImageView imageView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            var btnCamera = FindViewById<Button>(Resource.Id.btnCamera);
            imageView = FindViewById<ImageView>(Resource.Id.imageView);

            btnCamera.Click += BtnCamera_Click;
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            Bitmap bitmap = (Bitmap)data.Extras.Get("data");
            imageView.SetImageBitmap(bitmap);
        }

        private void BtnCamera_Click(object sender, System.EventArgs e)
        {
            Reco.GetInstance().Load("Resources/sampleRepository.bin");
            string name = Reco.GetInstance().GetName("Resources/imageToFind.jpeg");
            TextView textView = FindViewById<TextView>(Resource.Id.textView);
            textView.Text = name;
            //Intent intent = new Intent(MediaStore.ActionImageCapture);
            //StartActivityForResult(intent, 0);
        }
    }
}