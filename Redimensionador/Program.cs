using System.Drawing;

namespace Redimensionador
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando redimensionador");

            Thread thread = new Thread(Redimensionar);
            thread.Start();

            Console.ReadLine();
        }

        static void Redimensionar()
        {
            #region "Diretorios"
            string diretorioEntrada = "Arquivos_Entrada";
            string diretotioFinalizado = "Arquivos_Finalizados";
            string diretorioRedimensionado = "Arquivos_Redimensionados";



            if (!Directory.Exists(diretorioEntrada))
            {
                Directory.CreateDirectory(diretorioEntrada);
            }

            if (!Directory.Exists(diretotioFinalizado)) 
            { 
                Directory.CreateDirectory (diretotioFinalizado);
            }

            if (!Directory.Exists(diretorioRedimensionado))
            {
                Directory.CreateDirectory((diretorioRedimensionado));
            }
            #endregion

            FileStream fileStream;
            FileInfo fileInfo;

            int[] tamanhos = { 200, 400, 600 };


            while (true)
            {

               var arquivosEntrada = Directory.EnumerateFiles(diretorioEntrada);


                foreach (var arquivo in arquivosEntrada)
                {
                     fileStream = new FileStream(arquivo, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                     fileInfo = new FileInfo(arquivo);

                    foreach (var tamanho in tamanhos)
                    {
                        string caminho = Path.Combine(diretorioRedimensionado, $"{DateTime.Now.Millisecond}_{tamanho}_{fileInfo.Name}");

                        Redimensionador(Image.FromStream(fileStream), tamanho, caminho);
                    }

                   

                    fileStream.Close();

                    string caminhoFinalizado = Path.Combine(diretotioFinalizado, fileInfo.Name);
                    fileInfo.MoveTo(caminhoFinalizado);
            }

                Thread.Sleep(new TimeSpan(0, 0, 5));
            }
        }

        /// <summary>
        /// Redimensiona a imagem para a altura especificada mantendo a proporção.
        /// </summary>
        /// <param name="imagem">Imagem a ser redimensionada</param>
        /// <param name="altura">Altura que desejamos redimensionar</param>
        /// <param name="caminho">Caminho onde iremos gravar o arquivo redimensionado</param>
        /// <returns></returns>

        static void Redimensionador(Image imagem, int altura, string caminho)
        {
            double ratio = (double)altura / imagem.Height;
            int novaLargura = (int)(imagem.Width * ratio);
            int novaAltura = (int)(imagem.Height * ratio);

            Bitmap novaImage = new Bitmap(novaLargura, novaAltura);

            using(Graphics g = Graphics.FromImage(novaImage))
            {
                g.DrawImage(imagem, 0, 0, novaLargura, novaAltura);
            }

            novaImage.Save(caminho);
            novaImage.Dispose();
            imagem.Dispose();
        }
    }
}