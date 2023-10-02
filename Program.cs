using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace didaticos.redimensionador
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando o Redimensionador");
            Thread thread = new Thread(Redimensionar);
            thread.Start();
        }

        #region Redimensionar
        static void Redimensionar() 
        {

            string diretorioentrada = "Aquivos_Entrada";
            string diretorioredimensionado = "Aquivo_Redimensionado";
            string diretoriofinalizados = "Aquivos_Finalizados";

            if (!Directory.Exists(diretorioentrada))
            {
                Directory.CreateDirectory(diretorioentrada);
            }      
           
            if (!Directory.Exists(diretorioredimensionado))
            {
                Directory.CreateDirectory(diretorioredimensionado);
            }
            
            if (!Directory.Exists(diretoriofinalizados))
            {
                Directory.CreateDirectory(diretoriofinalizados);
            }

            FileStream fileStream;
            FileInfo fileInfo;
           
            while (true)
            {
                var arquivosEntrada = Directory.EnumerateFiles(diretorioentrada);
                int AlturaThumb200 = 200;
                int AlturaThumb100 = 100;

                foreach (var aquivo in arquivosEntrada)
                {

                    fileStream = new FileStream(aquivo, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    fileInfo = new FileInfo(aquivo);

                    string caminho200 = Environment.CurrentDirectory + @"\" + diretorioredimensionado + @"\200px" + DateTime.Now.Millisecond + fileInfo.Name;
                    string caminho100 = Environment.CurrentDirectory + @"\" + diretorioredimensionado + @"\100px" + DateTime.Now.Millisecond + fileInfo.Name;


                    Redimensionador(Image.FromStream(fileStream), AlturaThumb200, caminho200);
                    Redimensionador(Image.FromStream(fileStream), AlturaThumb100, caminho100);


                    fileStream.Close();

                    string caminhoFinalizado = Environment.CurrentDirectory + @"\" + diretoriofinalizados + @"\" + fileInfo.Name;
                    fileInfo.MoveTo(caminhoFinalizado);

                }
                    

                Thread.Sleep(new TimeSpan(0, 0, 3));
            }
        
        }
        #endregion

        static void Redimensionador(Image imagem, int altura, string caminho)
            
        {
            double ratio = (double)altura / imagem.Height;
            int novaLargura = (int)(imagem.Width * ratio);
            int novaAltura = (int)(imagem.Height * ratio);

            Bitmap novaImagem = new Bitmap(novaLargura, novaAltura);

            using (Graphics g = Graphics.FromImage(novaImagem)) 
            {
                g.DrawImage(imagem, 0, 0, novaLargura, novaAltura);
            }
            //Salvando Imagem
            novaImagem.Save(caminho);
            imagem.Dispose();
        }
    }
}
