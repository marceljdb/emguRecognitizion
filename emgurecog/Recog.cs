using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emgurecog
{
    public class Recog : IRecog
    {
        private static readonly double EIGEN_DISTANCE_THRESHOLD = 1000;

        private CascadeClassifier Face;

        private MCvTermCriteria TermCri;

        private FaceRecognizer recognizer;



        public void Trainer(int id, List<Bitmap> listaImagens)
        {
            FaceTrainer(id, listaImagens);
        }

        public bool Execute(Bitmap bitmap)
        {
            return MatchFaces(bitmap);
        }


        private Image<Gray, Byte> ConvertToGrayScale(Bitmap bitmap)
        {
            var imagemEntrada = new Image<Gray, byte>(bitmap);

            //Converter para escala em cinza
            return imagemEntrada.Convert<Gray, Byte>();
        }

        private List<Image<Gray, Byte>> DetectFaceTrainer(List<Bitmap> listaImagens)
        {
            Face = new CascadeClassifier(@"Lib\haarcascade_frontalface_default.xml");


            List<Image<Gray, Byte>> listaFaces = new List<Image<Gray, byte>>();
            foreach (var img in listaImagens)
            {

                //Converte para escala em cinza
                var grayImagem = ConvertToGrayScale(img);

                Rectangle[] facesDetected = Face.DetectMultiScale(grayImagem, 1.3, 3, new Size(50, 50), Size.Empty);


                if (facesDetected.Length > 0)
                {
                    listaFaces.Add(DetectAndResize(facesDetected[0], img));
                }

            }
            return listaFaces;
        }

        private Image<Gray, Byte> DetectAndResize(Rectangle face, Bitmap bitmap)
        {

            var imageEntrada = new Image<Bgr, Byte>(bitmap);
            Image<Gray, Byte> imageSaida = null;
            var result = imageEntrada.Copy(face).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            result._EqualizeHist();

            imageSaida = result;
            return imageSaida;
        }

        private void FaceTrainer(int id, List<Bitmap> listaImagens)
        {
            var listaFaces = DetectFaceTrainer(listaImagens);
            var termCriteria = new MCvTermCriteria(listaFaces.Count, 0.001);

            List<int> listaId = new List<int>();

            foreach (var img in listaFaces) { listaId.Add(id); }

            recognizer = new LBPHFaceRecognizer(1, 8, 8, 8, 80);//50
            recognizer.Train(listaFaces.ToArray(), listaId.ToArray());
        }

        private bool MatchFaces(Bitmap bitmap)
        {
            var imageReceived = DetectFaceTrainer(new List<Bitmap> { bitmap });

            if ((imageReceived == null) || (imageReceived.Count() == Decimal.Zero))
                return false;

            LBPHFaceRecognizer.PredictionResult ER = recognizer.Predict(imageReceived[0]);

            return (ER.Label > -1);
        }
    }
}
