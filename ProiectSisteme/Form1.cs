using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProiectSisteme
{
    public partial class Form1 : Form
    {
        private uint[] handleFir = new uint[4];
        private uint handlePolitie;
        private uint handleCatel;
        private uint handleBunica;
        private uint[] fir = new uint[4];
        private uint[] firCod = new uint[4];
        private IntPtr[] handleConversie = new IntPtr[4];
        private uint handleSupraveghetor;
        private uint firSupraveghetor;
        
        private uint firPolitie;
        private uint firBunica;
        private uint firCatel;

        private IntPtr evSemaforVerdeNordSud;
        private IntPtr evSemaforVerdeVestEst;
        private IntPtr evMergeSemaforul;

        private IntPtr evApasatButonCaine;
        private IntPtr evApasatButonBunica;

        private Random random = new Random();
        private int ctBunica = 0;
        private int ctCatel = 0;
        public Form1()
        {
            InitializeComponent();
            
        }

        public void semafoare()
        {
            int comutator = random.Next(1, 3);
            int mergeSemaforul = random.Next(1, 3);

            if (mergeSemaforul == 1)
            {

                WinApiClass.SetEvent(evMergeSemaforul);

                if (comutator == 1)
                {

                    FileStream fs = new System.IO.FileStream(@"./../../imaginiDesign/semaforVerde.png", FileMode.Open, FileAccess.Read);
                    semaforNordPoza.Image = Image.FromStream(fs);
                    fs.Close();

                    FileStream fs2 = new System.IO.FileStream(@"./../../imaginiDesign/semaforVerde.png", FileMode.Open, FileAccess.Read);
                    semaforSudPoza.Image = Image.FromStream(fs2);
                    fs2.Close();

                    WinApiClass.SetEvent(evSemaforVerdeNordSud);

                }

                else
                {

                    FileStream fs = new System.IO.FileStream(@"./../../imaginiDesign/semaforVerde.png", FileMode.Open, FileAccess.Read);
                    semaforVestPoza.Image = Image.FromStream(fs);
                    fs.Close();

                    FileStream fs2 = new System.IO.FileStream(@"./../../imaginiDesign/semaforVerde.png", FileMode.Open, FileAccess.Read);
                    semaforEstPoza.Image = Image.FromStream(fs2);
                    fs2.Close();

                    WinApiClass.SetEvent(evSemaforVerdeVestEst);

                }
            }

            if (mergeSemaforul == 2)
            {

                FileStream fs = new System.IO.FileStream(@"./../../imaginiDesign/semaforGalben.png", FileMode.Open, FileAccess.Read);
                semaforNordPoza.Image = Image.FromStream(fs);
                fs.Close();

                FileStream fs2 = new System.IO.FileStream(@"./../../imaginiDesign/semaforGalben.png", FileMode.Open, FileAccess.Read);
                semaforSudPoza.Image = Image.FromStream(fs2);
                fs2.Close();

                FileStream fs3 = new System.IO.FileStream(@"./../../imaginiDesign/semaforGalben.png", FileMode.Open, FileAccess.Read);
                semaforVestPoza.Image = Image.FromStream(fs3);
                fs3.Close();

                FileStream fs4 = new System.IO.FileStream(@"./../../imaginiDesign/semaforGalben.png", FileMode.Open, FileAccess.Read);
                semaforEstPoza.Image = Image.FromStream(fs4);
                fs4.Close();

                WinApiClass.SetEvent(evSemaforVerdeVestEst);

            }
        }

        public uint threadMasinaRosie(IntPtr a)
        {
            int vehicul = masinaRosiePoza.Left;
            
            uint timpAsteptareSemaforVE = WinApiClass.WaitForSingleObject(evSemaforVerdeVestEst, 15000);
            uint timpAsteptareSemaforNS = WinApiClass.WaitForSingleObject(evSemaforVerdeNordSud, 1000);
            uint timpAsteptareMergeSemafor = WinApiClass.WaitForSingleObject(evMergeSemaforul, 1000);
            uint timpAsteptareApasatButonCaine = 0;
            int verificare = 0;

            if (timpAsteptareSemaforVE == WinApiClass.WAIT_OBJECT_0)
            {

                for (var i = 0; i <= 50; i++)
                {

                    if (i >= 10 && verificare == 0)
                    {
                        timpAsteptareApasatButonCaine = WinApiClass.WaitForSingleObject(evApasatButonCaine, 1000);
                        verificare = 1;
                    }

                    if (verificare == 1  && timpAsteptareApasatButonCaine == WinApiClass.WAIT_OBJECT_0)
                    {
                        WinApiClass.SuspendThread((IntPtr)handleFir[0]);
                        verificare = 2;
                    }

                    masinaRosiePoza.Invoke(new Action(() => masinaRosiePoza.Left = vehicul)); 
                    vehicul -= i;
                    Thread.Sleep(110);

                }
                
                WinApiClass.ResetEvent(evSemaforVerdeVestEst);
                WinApiClass.SetEvent(evSemaforVerdeNordSud);


                if (timpAsteptareMergeSemafor == WinApiClass.WAIT_OBJECT_0)
                {
                    FileStream fs = new System.IO.FileStream(@"./../../imaginiDesign/semaforRosu.png", FileMode.Open, FileAccess.Read);
                    semaforVestPoza.Image = Image.FromStream(fs);
                    fs.Close();

                    FileStream fs2 = new System.IO.FileStream(@"./../../imaginiDesign/semaforRosu.png", FileMode.Open, FileAccess.Read);
                    semaforEstPoza.Image = Image.FromStream(fs2);
                    fs2.Close();

                    FileStream fs3 = new System.IO.FileStream(@"./../../imaginiDesign/semaforVerde.png", FileMode.Open, FileAccess.Read);
                    semaforNordPoza.Image = Image.FromStream(fs3);
                    fs3.Close();

                    FileStream fs4 = new System.IO.FileStream(@"./../../imaginiDesign/semaforVerde.png", FileMode.Open, FileAccess.Read);
                    semaforSudPoza.Image = Image.FromStream(fs4);
                    fs4.Close();
                }

                else
                {
                    FileStream fs = new System.IO.FileStream(@"./../../imaginiDesign/semaforGalben.png", FileMode.Open, FileAccess.Read);
                    semaforNordPoza.Image = Image.FromStream(fs);
                    fs.Close();

                    FileStream fs2 = new System.IO.FileStream(@"./../../imaginiDesign/semaforGalben.png", FileMode.Open, FileAccess.Read);
                    semaforSudPoza.Image = Image.FromStream(fs2);
                    fs2.Close();

                    FileStream fs3 = new System.IO.FileStream(@"./../../imaginiDesign/semaforGalben.png", FileMode.Open, FileAccess.Read);
                    semaforVestPoza.Image = Image.FromStream(fs3);
                    fs3.Close();

                    FileStream fs4 = new System.IO.FileStream(@"./../../imaginiDesign/semaforGalben.png", FileMode.Open, FileAccess.Read);
                    semaforEstPoza.Image = Image.FromStream(fs4);
                    fs4.Close();
                }


                return 0;

            }

            else if (timpAsteptareSemaforNS == WinApiClass.WAIT_OBJECT_0)
            {
                
                if (timpAsteptareSemaforVE == WinApiClass.WAIT_OBJECT_0)
                {
                    

                    WinApiClass.SetEvent(evSemaforVerdeVestEst);
                    for (var i = 0; i <= 50; i++)
                    {

                        if (i >= 10 && verificare == 0)
                        {
                            timpAsteptareApasatButonCaine = WinApiClass.WaitForSingleObject(evApasatButonCaine, 1000);
                            verificare = 1;
                        }

                        if (verificare == 1 && timpAsteptareApasatButonCaine == WinApiClass.WAIT_OBJECT_0)
                        {
                            WinApiClass.SuspendThread((IntPtr)handleFir[0]);
                            verificare = 2;
                        }

                        masinaRosiePoza.Invoke(new Action(() => masinaRosiePoza.Left = vehicul)); 
                        vehicul -= i;
                        Thread.Sleep(110);
                    }
                    
                    WinApiClass.ResetEvent(evSemaforVerdeVestEst);
                    WinApiClass.SetEvent(evSemaforVerdeNordSud);

                    if (timpAsteptareMergeSemafor == WinApiClass.WAIT_OBJECT_0)
                    {
                        FileStream fs = new System.IO.FileStream(@"./../../imaginiDesign/semaforRosu.png", FileMode.Open, FileAccess.Read);
                        semaforVestPoza.Image = Image.FromStream(fs);
                        fs.Close();

                        FileStream fs2 = new System.IO.FileStream(@"./../../imaginiDesign/semaforRosu.png", FileMode.Open, FileAccess.Read);
                        semaforEstPoza.Image = Image.FromStream(fs2);
                        fs2.Close();

                        FileStream fs3 = new System.IO.FileStream(@"./../../imaginiDesign/semaforVerde.png", FileMode.Open, FileAccess.Read);
                        semaforNordPoza.Image = Image.FromStream(fs3);
                        fs3.Close();

                        FileStream fs4 = new System.IO.FileStream(@"./../../imaginiDesign/semaforVerde.png", FileMode.Open, FileAccess.Read);
                        semaforSudPoza.Image = Image.FromStream(fs4);
                        fs4.Close();
                    }

                    else
                    {
                        FileStream fs = new System.IO.FileStream(@"./../../imaginiDesign/semaforGalben.png", FileMode.Open, FileAccess.Read);
                        semaforNordPoza.Image = Image.FromStream(fs);
                        fs.Close();

                        FileStream fs2 = new System.IO.FileStream(@"./../../imaginiDesign/semaforGalben.png", FileMode.Open, FileAccess.Read);
                        semaforSudPoza.Image = Image.FromStream(fs2);
                        fs2.Close();

                        FileStream fs3 = new System.IO.FileStream(@"./../../imaginiDesign/semaforGalben.png", FileMode.Open, FileAccess.Read);
                        semaforVestPoza.Image = Image.FromStream(fs3);
                        fs3.Close();

                        FileStream fs4 = new System.IO.FileStream(@"./../../imaginiDesign/semaforGalben.png", FileMode.Open, FileAccess.Read);
                        semaforEstPoza.Image = Image.FromStream(fs4);
                        fs4.Close();
                    }

                    return 0;
                }

                else if (timpAsteptareSemaforVE == WinApiClass.WAIT_TIMEOUT)
                {
                    return 100;
                }

            }

            return 100;
        }

        public uint threadTaxi(IntPtr a)
        {
            SoundPlayer claxon = new SoundPlayer(@"./../../claxon.wav");
            uint timpAsteptareSemaforVE = WinApiClass.WaitForSingleObject(evSemaforVerdeVestEst, 15000);
            uint timpAsteptareSemaforNS = WinApiClass.WaitForSingleObject(evSemaforVerdeNordSud, 1000);
            uint timpAsteptareMergeSemafor = WinApiClass.WaitForSingleObject(evMergeSemaforul, 1000);
            uint timpAsteptareApasatButonCaine=0;
            int verificare = 0;

            int vehicul = taxiPoza.Left;
            int taxi = random.Next(1, 3);

            if (timpAsteptareSemaforVE == WinApiClass.WAIT_OBJECT_0)
            {

                if (taxi == 1)
                {
                    claxon.Play();
                }

                for (var i = 0; i <= 50; i++)
                {
                    taxiPoza.Invoke(new Action(() => taxiPoza.Left = vehicul)); 
                    vehicul += i;

                    if (i >= 28 && verificare == 0) {
                        timpAsteptareApasatButonCaine = WinApiClass.WaitForSingleObject(evApasatButonCaine, 1000);
                        verificare = 1;
                    }

                    if ( verificare== 1 && timpAsteptareApasatButonCaine == WinApiClass.WAIT_OBJECT_0)
                    {
                        WinApiClass.SuspendThread((IntPtr)handleFir[1]);
                        verificare = 2;
                    }
                     

                    Thread.Sleep(110);
                }


                if (taxi == 1)
                {
                    return 777;
                }

                else
                {                    
                    return 1;
                }

            }
            else if (timpAsteptareSemaforNS == WinApiClass.WAIT_OBJECT_0)
            {
                
                if (timpAsteptareSemaforVE == WinApiClass.WAIT_OBJECT_0)
                {
                    if (taxi == 1)
                    {
                        claxon.Play();
                    }

                    for (var i = 0; i <= 50; i++)
                    {
                        taxiPoza.Invoke(new Action(() => taxiPoza.Left = vehicul)); 
                        vehicul += i;

                        if (i >= 28 && verificare == 0)
                        {
                            timpAsteptareApasatButonCaine = WinApiClass.WaitForSingleObject(evApasatButonCaine, 1000);
                            verificare = 1;
                        }

                        if (verificare == 1 && timpAsteptareApasatButonCaine == WinApiClass.WAIT_OBJECT_0)
                        {
                            WinApiClass.SuspendThread((IntPtr)handleFir[1]);
                            verificare = 2;
                        }

                        Thread.Sleep(110);
                    }

                    if (taxi == 1)
                    {
                        return 777;
                    }

                    else
                    {
                        return 1;
                    }

                }

                else if (timpAsteptareSemaforVE == WinApiClass.WAIT_TIMEOUT)
                {
                    return 101;
                }
            }



            return 101;
        }



        public uint threadMasinaAlba(IntPtr a)
        {
            uint timpAsteptareSemaforVE = WinApiClass.WaitForSingleObject(evSemaforVerdeVestEst, 1000);
            uint timpAsteptareSemaforNS = WinApiClass.WaitForSingleObject(evSemaforVerdeNordSud, 15000);
            uint timpAsteptareMergeSemafor = WinApiClass.WaitForSingleObject(evMergeSemaforul, 1000);
            uint timpAsteptareApasatButonBunica = 0;
            uint timpAsteptareApasatButonCaine = 0;

            int vehicul = masinaAlbaPoza.Top;
            int directie = random.Next(1, 3);
            int verificareCaine = 0;
            int verificareBunica = 0;

            if (timpAsteptareSemaforNS == WinApiClass.WAIT_OBJECT_0)
            {
                if (directie == 1)
                {
                    for (var i = 0; i <= 50; i++)
                    {
                        masinaAlbaPoza.Invoke(new Action(() => masinaAlbaPoza.Top = vehicul)); 
                        vehicul -= i;

                        if (i >= 26 && verificareBunica == 0)
                        {
                            timpAsteptareApasatButonBunica = WinApiClass.WaitForSingleObject(evApasatButonBunica, 1000);
                            verificareBunica = 1;
                        }

                        if (verificareBunica == 1 && timpAsteptareApasatButonBunica == WinApiClass.WAIT_OBJECT_0)
                        {
                            WinApiClass.SuspendThread((IntPtr)handleFir[2]);
                            verificareBunica = 2;
                        }

                        Thread.Sleep(110);
                    }
                }

                if (directie == 2)
                {
                    for (var i = 0; i <= 18; i++)
                    {
                        masinaAlbaPoza.Invoke(new Action(() => masinaAlbaPoza.Top = vehicul)); 
                        vehicul -= i;

                        Thread.Sleep(110);
                    }

                    Image img = masinaAlbaPoza.Image;
                    img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    masinaAlbaPoza.Image = img;

                    for (var i = 0; i <= 40; i++)
                    {
                        masinaAlbaPoza.Invoke(new Action(() => masinaAlbaPoza.Left = vehicul)); 
                        vehicul += i;

                        if (i >= 14 && verificareCaine == 0)
                        {
                            timpAsteptareApasatButonCaine = WinApiClass.WaitForSingleObject(evApasatButonCaine, 1000);
                            verificareCaine = 1;
                        }

                        if (verificareCaine == 1 && timpAsteptareApasatButonCaine == WinApiClass.WAIT_OBJECT_0)
                        {
                            WinApiClass.SuspendThread((IntPtr)handleFir[2]);
                            verificareCaine = 2;
                        }

                        Thread.Sleep(110);
                    }
                }

                WinApiClass.ResetEvent(evSemaforVerdeNordSud);
                WinApiClass.SetEvent(evSemaforVerdeVestEst);

                if (timpAsteptareMergeSemafor == WinApiClass.WAIT_OBJECT_0)
                {
                    FileStream fs = new System.IO.FileStream(@"./../../imaginiDesign/semaforVerde.png", FileMode.Open, FileAccess.Read);
                    semaforVestPoza.Image = Image.FromStream(fs);
                    fs.Close();

                    FileStream fs2 = new System.IO.FileStream(@"./../../imaginiDesign/semaforVerde.png", FileMode.Open, FileAccess.Read);
                    semaforEstPoza.Image = Image.FromStream(fs2);
                    fs2.Close();

                    FileStream fs3 = new System.IO.FileStream(@"./../../imaginiDesign/semaforRosu.png", FileMode.Open, FileAccess.Read);
                    semaforNordPoza.Image = Image.FromStream(fs3);
                    fs3.Close();

                    FileStream fs4 = new System.IO.FileStream(@"./../../imaginiDesign/semaforRosu.png", FileMode.Open, FileAccess.Read);
                    semaforSudPoza.Image = Image.FromStream(fs4);
                    fs4.Close();
                }

                else
                {
                    FileStream fs = new System.IO.FileStream(@"./../../imaginiDesign/semaforGalben.png", FileMode.Open, FileAccess.Read);
                    semaforNordPoza.Image = Image.FromStream(fs);
                    fs.Close();

                    FileStream fs2 = new System.IO.FileStream(@"./../../imaginiDesign/semaforGalben.png", FileMode.Open, FileAccess.Read);
                    semaforSudPoza.Image = Image.FromStream(fs2);
                    fs2.Close();

                    FileStream fs3 = new System.IO.FileStream(@"./../../imaginiDesign/semaforGalben.png", FileMode.Open, FileAccess.Read);
                    semaforVestPoza.Image = Image.FromStream(fs3);
                    fs3.Close();

                    FileStream fs4 = new System.IO.FileStream(@"./../../imaginiDesign/semaforGalben.png", FileMode.Open, FileAccess.Read);
                    semaforEstPoza.Image = Image.FromStream(fs4);
                    fs4.Close();
                }

                return 2;
            }

            else if (timpAsteptareSemaforVE == WinApiClass.WAIT_OBJECT_0)
            {
                if (timpAsteptareSemaforNS == WinApiClass.WAIT_OBJECT_0)
                {
                    WinApiClass.SetEvent(evSemaforVerdeNordSud);
                    if (directie == 1)
                    {
                        for (var i = 0; i <= 50; i++)
                        {
                            masinaAlbaPoza.Invoke(new Action(() => masinaAlbaPoza.Top = vehicul)); 
                            vehicul -= i;

                            if (i >= 26 && verificareBunica == 0)
                            {
                                timpAsteptareApasatButonBunica = WinApiClass.WaitForSingleObject(evApasatButonBunica, 1000);
                                verificareBunica = 1;
                            }

                            if (verificareBunica == 1 && timpAsteptareApasatButonBunica == WinApiClass.WAIT_OBJECT_0)
                            {
                                WinApiClass.SuspendThread((IntPtr)handleFir[2]);
                                verificareBunica = 2;
                            }

                            Thread.Sleep(110);
                        }
                    }

                    if (directie == 2)
                    {
                        for (var i = 0; i <= 18; i++)
                        {
                            masinaAlbaPoza.Invoke(new Action(() => masinaAlbaPoza.Top = vehicul)); 
                            vehicul -= i;                         
                            Thread.Sleep(110);
                        }

                        Image img = masinaAlbaPoza.Image;
                        img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        masinaAlbaPoza.Image = img;

                        for (var i = 0; i <= 40; i++)
                        {
                            masinaAlbaPoza.Invoke(new Action(() => masinaAlbaPoza.Left = vehicul)); 
                            vehicul += i;

                            if (i >= 14 && verificareCaine == 0)
                            {
                                timpAsteptareApasatButonCaine = WinApiClass.WaitForSingleObject(evApasatButonCaine, 1000);
                                verificareCaine = 1;
                            }

                            if (verificareCaine == 1 && timpAsteptareApasatButonCaine == WinApiClass.WAIT_OBJECT_0)
                            {
                                WinApiClass.SuspendThread((IntPtr)handleFir[2]);
                                verificareCaine = 2;
                            }

                            Thread.Sleep(110);
                        }
                    }

                    WinApiClass.ResetEvent(evSemaforVerdeNordSud);
                    WinApiClass.SetEvent(evSemaforVerdeVestEst);

                    if (timpAsteptareMergeSemafor == WinApiClass.WAIT_OBJECT_0)
                    {
                        FileStream fs = new System.IO.FileStream(@"./../../imaginiDesign/semaforVerde.png", FileMode.Open, FileAccess.Read);
                        semaforVestPoza.Image = Image.FromStream(fs);
                        fs.Close();

                        FileStream fs2 = new System.IO.FileStream(@"./../../imaginiDesign/semaforVerde.png", FileMode.Open, FileAccess.Read);
                        semaforEstPoza.Image = Image.FromStream(fs2);
                        fs2.Close();

                        FileStream fs3 = new System.IO.FileStream(@"./../../imaginiDesign/semaforRosu.png", FileMode.Open, FileAccess.Read);
                        semaforNordPoza.Image = Image.FromStream(fs3);
                        fs3.Close();

                        FileStream fs4 = new System.IO.FileStream(@"./../../imaginiDesign/semaforRosu.png", FileMode.Open, FileAccess.Read);
                        semaforSudPoza.Image = Image.FromStream(fs4);
                        fs4.Close();
                    }

                    else
                    {
                        FileStream fs = new System.IO.FileStream(@"./../../imaginiDesign/semaforGalben.png", FileMode.Open, FileAccess.Read);
                        semaforNordPoza.Image = Image.FromStream(fs);
                        fs.Close();

                        FileStream fs2 = new System.IO.FileStream(@"./../../imaginiDesign/semaforGalben.png", FileMode.Open, FileAccess.Read);
                        semaforSudPoza.Image = Image.FromStream(fs2);
                        fs2.Close();

                        FileStream fs3 = new System.IO.FileStream(@"./../../imaginiDesign/semaforGalben.png", FileMode.Open, FileAccess.Read);
                        semaforVestPoza.Image = Image.FromStream(fs3);
                        fs3.Close();

                        FileStream fs4 = new System.IO.FileStream(@"./../../imaginiDesign/semaforGalben.png", FileMode.Open, FileAccess.Read);
                        semaforEstPoza.Image = Image.FromStream(fs4);
                        fs4.Close();
                    }

                    return 2;
                }

                else if (timpAsteptareSemaforNS == WinApiClass.WAIT_TIMEOUT)
                {
                    return 102;
                }
            }

            return 102;
        }

        public uint threadCamion(IntPtr a)
        {
            uint timpAsteptareSemaforVE = WinApiClass.WaitForSingleObject(evSemaforVerdeVestEst, 1000);
            uint timpAsteptareSemaforNS = WinApiClass.WaitForSingleObject(evSemaforVerdeNordSud, 15000);
            uint timpAsteptareMergeSemafor = WinApiClass.WaitForSingleObject(evMergeSemaforul, 1000);
            uint timpAsteptareApasatButonCaine = 0;
            uint timpAsteptareApasatButonBunica = 0;
            int verificareCaine = 0;
            int verificareBunica = 0;
            int vehicul = camionPoza.Top;

            int directie = random.Next(1, 4);

            if (timpAsteptareSemaforNS == WinApiClass.WAIT_OBJECT_0)
            {
                if (directie == 1)
                {
                    for (var i = 0; i <= 50; i++)
                    {
                        camionPoza.Invoke(new Action(() => camionPoza.Top = vehicul)); 
                        vehicul += i;
                        Console.WriteLine(i);
                        if (i >= 5 && verificareBunica == 0)
                        {
                            timpAsteptareApasatButonBunica = WinApiClass.WaitForSingleObject(evApasatButonBunica, 1000);
                            verificareBunica = 1;
                        }

                        if (verificareBunica == 1 && timpAsteptareApasatButonBunica == WinApiClass.WAIT_OBJECT_0)
                        {
                            WinApiClass.SuspendThread((IntPtr)handleFir[3]);
                            verificareBunica = 2;
                        }

                        Thread.Sleep(110);
                    }
                }


                if (directie == 2)
                {
                    for (var i = 0; i <= 22; i++)
                    {
                        camionPoza.Invoke(new Action(() => camionPoza.Top = vehicul)); 
                        vehicul += i;

                        if (i >= 5 && verificareBunica == 0)
                        {
                            timpAsteptareApasatButonBunica = WinApiClass.WaitForSingleObject(evApasatButonBunica, 1000);
                            verificareBunica = 1;
                        }

                        if (verificareBunica == 1 && timpAsteptareApasatButonBunica == WinApiClass.WAIT_OBJECT_0)
                        {
                            WinApiClass.SuspendThread((IntPtr)handleFir[3]);
                            verificareBunica = 2;
                        }

                        Thread.Sleep(110);
                    }

                    Image img = camionPoza.Image;
                    img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    camionPoza.Image = img;

                    for (var i = 0; i <= 40; i++)
                    {
                        camionPoza.Invoke(new Action(() => camionPoza.Left = vehicul)); 
                        vehicul -= i;

                        Thread.Sleep(110);
                    }
                }

                if (directie == 3)
                {
                    for (var i = 0; i <= 22; i++)
                    {
                        camionPoza.Invoke(new Action(() => camionPoza.Top = vehicul)); 
                        vehicul += i;

                        if (i >= 5 && verificareBunica == 0)
                        {
                            timpAsteptareApasatButonBunica = WinApiClass.WaitForSingleObject(evApasatButonBunica, 1000);
                            verificareBunica = 1;
                        }

                        if (verificareBunica == 1 && timpAsteptareApasatButonBunica == WinApiClass.WAIT_OBJECT_0)
                        {
                            WinApiClass.SuspendThread((IntPtr)handleFir[3]);
                            verificareBunica = 2;
                        }

                        Thread.Sleep(110);
                    }

                    for (var i = 0; i <= 12; i++)
                    {
                        camionPoza.Invoke(new Action(() => camionPoza.Top = vehicul)); 
                        vehicul += i;

                        

                        Thread.Sleep(110);
                    }

                    Image img = camionPoza.Image;
                    img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    camionPoza.Image = img;

                    for (var i = 0; i <= 40; i++)
                    {
                        camionPoza.Invoke(new Action(() => camionPoza.Left = vehicul)); 
                        vehicul += i;

                        if (i >= 1 && verificareCaine == 0)
                        {
                            timpAsteptareApasatButonCaine = WinApiClass.WaitForSingleObject(evApasatButonCaine, 1000);
                            verificareCaine = 1;
                        }

                        if (verificareCaine == 1 && timpAsteptareApasatButonCaine == WinApiClass.WAIT_OBJECT_0)
                        {
                            WinApiClass.SuspendThread((IntPtr)handleFir[3]);
                            verificareCaine = 2;
                        }

                        Thread.Sleep(110);
                    }
                }

                return 3;
            }
            else if (timpAsteptareSemaforVE == WinApiClass.WAIT_OBJECT_0)
            {
                if (timpAsteptareSemaforNS == WinApiClass.WAIT_OBJECT_0)
                {
                    if (directie == 1)
                    {
                        for (var i = 0; i <= 50; i++)
                        {
                            camionPoza.Invoke(new Action(() => camionPoza.Top = vehicul)); 
                            vehicul += i;

                            if (i >= 5 && verificareBunica == 0)
                            {
                                timpAsteptareApasatButonBunica = WinApiClass.WaitForSingleObject(evApasatButonBunica, 1000);
                                verificareBunica = 1;
                            }

                            if (verificareBunica == 1 && timpAsteptareApasatButonBunica == WinApiClass.WAIT_OBJECT_0)
                            {
                                WinApiClass.SuspendThread((IntPtr)handleFir[3]);
                                verificareBunica = 2;
                            }

                            Thread.Sleep(110);
                        }
                    }


                    if (directie == 2)
                    {
                        for (var i = 0; i <= 22; i++)
                        {
                            camionPoza.Invoke(new Action(() => camionPoza.Top = vehicul)); 
                            vehicul += i;

                            Thread.Sleep(110);
                        }

                        Image img = camionPoza.Image;
                        img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        camionPoza.Image = img;

                        for (var i = 0; i <= 40; i++)
                        {
                            camionPoza.Invoke(new Action(() => camionPoza.Left = vehicul)); 
                            vehicul -= i;

                            Thread.Sleep(110);
                        }
                    }

                    if (directie == 3)
                    {
                        for (var i = 0; i <= 22; i++)
                        {
                            camionPoza.Invoke(new Action(() => camionPoza.Top = vehicul)); 
                            vehicul += i;

                            Thread.Sleep(110);
                        }

                        for (var i = 0; i <= 12; i++)
                        {
                            camionPoza.Invoke(new Action(() => camionPoza.Top = vehicul)); 
                            vehicul += i;

                            Thread.Sleep(110);
                        }

                        Image img = camionPoza.Image;
                        img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        camionPoza.Image = img;

                        for (var i = 0; i <= 40; i++)
                        {
                            camionPoza.Invoke(new Action(() => camionPoza.Left = vehicul)); 
                            vehicul += i;

                            if (i >= 1 && verificareCaine == 0)
                            {
                                timpAsteptareApasatButonCaine = WinApiClass.WaitForSingleObject(evApasatButonCaine, 1000);
                                verificareCaine = 1;
                            }

                            if (verificareCaine == 1 && timpAsteptareApasatButonCaine == WinApiClass.WAIT_OBJECT_0)
                            {
                                WinApiClass.SuspendThread((IntPtr)handleFir[3]);
                                verificareCaine = 2;
                            }

                            Thread.Sleep(110);
                        }
                    }
                    return 3;
                }

                else if (timpAsteptareSemaforNS == WinApiClass.WAIT_TIMEOUT)
                {
                    return 103;
                }
            }

            return 103;
        }

        public uint threadPolitie(IntPtr a)
        {

            int urgenta = random.Next(1, 4);

            if (urgenta == 1)
            {
                FileStream fs = new System.IO.FileStream(@"./../../imaginiDesign/ambulanta.png", FileMode.Open, FileAccess.Read);
                politiePoza.Image = Image.FromStream(fs);
                fs.Close();
            }

            else if (urgenta == 2)
            {
                FileStream fs2 = new System.IO.FileStream(@"./../../imaginiDesign/politie.png", FileMode.Open, FileAccess.Read);
                politiePoza.Image = Image.FromStream(fs2);
                fs2.Close();
            }

            else
            {
                FileStream fs3 = new System.IO.FileStream(@"./../../imaginiDesign/masinaPomp.png", FileMode.Open, FileAccess.Read);
                politiePoza.Image = Image.FromStream(fs3);
                fs3.Close();
            }

            for (var i = 0; i < handleFir.Length; i++)
            {
                WinApiClass.SuspendThread((IntPtr)handleFir[i]);
            }

            int vehicul = politiePoza.Left;

            for (var i = 0; i <= 50; i++)
            {
                politiePoza.Invoke(new Action(() => politiePoza.Left = vehicul)); 
                vehicul -= i;
                Thread.Sleep(110);
            }

            for (var i = 0; i < handleFir.Length; i++)
            {
                WinApiClass.ResumeThread((IntPtr)handleFir[i]);
            }

            if (urgenta == 1)
            {
                return 112;
            }

            else if (urgenta == 2)
            {
                return 1120;
            }

            else
            {
                return 1121;
            }

        }

        public uint threadBunica(IntPtr a)
        {
            int bunica = bunicaPoza.Left;

            if (ctBunica == 0) { 
            for (var i = 0; i <= 23; i++)
            {
                bunicaPoza.Invoke(new Action(() => bunicaPoza.Left = bunica)); 
                bunica += i;
                Thread.Sleep(180);
            }

            WinApiClass.ResetEvent(evApasatButonBunica);
            WinApiClass.ResumeThread((IntPtr)handleFir[3]);
            WinApiClass.ResumeThread((IntPtr)handleFir[2]);

            Image img = bunicaPoza.Image;
            img.RotateFlip(RotateFlipType.Rotate180FlipY);
            bunicaPoza.Image = img;
                ctBunica++;
            }

            else if (ctBunica == 1)
            {
                for (var i = 0; i <= 23; i++)
                {
                    bunicaPoza.Invoke(new Action(() => bunicaPoza.Left = bunica));
                    bunica -= i;
                    Thread.Sleep(180);
                }

                WinApiClass.ResetEvent(evApasatButonBunica);
                WinApiClass.ResumeThread((IntPtr)handleFir[3]);
                WinApiClass.ResumeThread((IntPtr)handleFir[2]);

                Image img = bunicaPoza.Image;
                img.RotateFlip(RotateFlipType.Rotate180FlipY);
                bunicaPoza.Image = img;
                ctBunica=0;
            }

            return 70;
        }

        public uint threadCatel(IntPtr a)
        {
            int catel = catelPoza.Top;

            if (ctCatel == 0)
            {
                for (var i = 0; i <= 23; i++)
                {
                    catelPoza.Invoke(new Action(() => catelPoza.Top = catel));
                    catel += i;
                    Thread.Sleep(170);
                }

                WinApiClass.ResetEvent(evApasatButonCaine);

                WinApiClass.ResumeThread((IntPtr)handleFir[0]);
                WinApiClass.ResumeThread((IntPtr)handleFir[1]);
                WinApiClass.ResumeThread((IntPtr)handleFir[2]);
                WinApiClass.ResumeThread((IntPtr)handleFir[3]);
                ctCatel++;
            }

            else if (ctCatel == 1)
            {
                for (var i = 0; i <= 23; i++)
                {
                    catelPoza.Invoke(new Action(() => catelPoza.Top = catel));
                    catel -= i;
                    Thread.Sleep(170);
                }

                WinApiClass.ResetEvent(evApasatButonCaine);

                WinApiClass.ResumeThread((IntPtr)handleFir[0]);
                WinApiClass.ResumeThread((IntPtr)handleFir[1]);
                WinApiClass.ResumeThread((IntPtr)handleFir[2]);
                WinApiClass.ResumeThread((IntPtr)handleFir[3]);
                ctCatel = 0;
            }

            return 7;           
        }

        public uint threadSupraveghetor(IntPtr a)
        {
            uint rezultat = WinApiClass.WaitForMultipleObjects(4, handleConversie, true, 50000);

            if (rezultat == WinApiClass.WAIT_OBJECT_0)
            {
                for (var i = 0; i < handleConversie.Length; i++)
                {
                    WinApiClass.GetExitCodeThread((uint)handleConversie[i], out firCod[i]);
                }
                WinApiClass.GetExitCodeThread(handlePolitie, out firPolitie);
                WinApiClass.GetExitCodeThread(handleCatel, out firCatel);
                WinApiClass.GetExitCodeThread(handleBunica, out firBunica);
            }

            string coduri = "";

            for (var i = 0; i < handleFir.Length; i++)
            {
                //coduri += "Vehiculul " + (i + 1) + " cod de iesire = " + firCod[i].ToString() + "\r\n";
                
                if (i == 0)
                {
                    coduri += "the red car crossed the street" + "\r\n";
                }

                if (i == 1)
                {
                    if (firCod[i] == 1)
                    { 
                        coduri += "the polite taxi driver crossed the street" + "\r\n";
                    }

                    else
                    {
                        coduri += "the rude taxi driver crossed the street" + "\r\n";
                    }

                }

                if (i == 2)
                {
                    coduri += "the white car crossed the street" + "\r\n";
                }

                if (i == 3)
                {
                    coduri += "the truck crossed the street" + "\r\n";
                }

                coduri += "Exit Code = " + firCod[i].ToString() + "\r\n";

            }

            if (firPolitie==112)
            {
                coduri += "the ambulance crossed the street"+ "\r\n" + " Exit Code = " + firPolitie.ToString() + "\r\n";
            }

            if (firPolitie == 1120)
            {
                coduri += "the police car crossed the street" + "\r\n" + " Exit Code = " + firPolitie.ToString() + "\r\n";
            }

            if (firPolitie == 1121)
            {
                coduri += "the fire truck crossed the street" + "\r\n" + " Exit Code = " + firPolitie.ToString() + "\r\n";
            }

            if (firCatel == 7)
            {
                coduri += "the dog crossed the street" + "\r\n" + " Exit Code = " + firCatel.ToString() + "\r\n";
            }

            if (firBunica == 70)
            {
                coduri += "the grandma crossed the street" + "\r\n" + " Exit Code = " + firBunica.ToString() + "\r\n";
            }

            DisplayTxtBox.Invoke(new Action(() => DisplayTxtBox.Text += coduri)); 

            return 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
                 
            evSemaforVerdeNordSud = WinApiClass.CreateEvent(IntPtr.Zero, true, false, null);
            evSemaforVerdeVestEst = WinApiClass.CreateEvent(IntPtr.Zero, true, false, null);
            evMergeSemaforul = WinApiClass.CreateEvent(IntPtr.Zero, true, false, null);

            semafoare();

            handleFir[0] = WinApiClass.CreateThread(IntPtr.Zero, 0, threadMasinaRosie, IntPtr.Zero, 0, out fir[0]);
            handleFir[1] = WinApiClass.CreateThread(IntPtr.Zero, 0, threadTaxi, IntPtr.Zero, 0, out fir[1]);
            handleFir[2] = WinApiClass.CreateThread(IntPtr.Zero, 0, threadMasinaAlba, IntPtr.Zero, 0, out fir[2]);
            handleFir[3] = WinApiClass.CreateThread(IntPtr.Zero, 0, threadCamion, IntPtr.Zero, 0, out fir[3]);

            for (var i = 0; i < handleFir.Length; i++)
            {
                handleConversie[i] = (IntPtr)handleFir[i];
            }

            handleSupraveghetor = WinApiClass.CreateThread(IntPtr.Zero, 0, threadSupraveghetor, IntPtr.Zero, 0, out firSupraveghetor);

            btnStart.Visible = false;
            
        }

        private void politieBtn_Click(object sender, EventArgs e)
        {
            btnPolitie.Visible = false;

            SoundPlayer sirena = new SoundPlayer(@"./../../sirena.wav");
            sirena.Play();
            handlePolitie=WinApiClass.CreateThread(IntPtr.Zero, 0, threadPolitie, IntPtr.Zero, 0, out firPolitie);
            

        }

        private void btnBunica_Click(object sender, EventArgs e)
        {
            evApasatButonBunica = WinApiClass.CreateEvent(IntPtr.Zero, true, true, null);
            handleBunica =WinApiClass.CreateThread(IntPtr.Zero, 0, threadBunica, IntPtr.Zero, 0, out firBunica);
        }

        private void btnCatel_Click(object sender, EventArgs e)
        {
            evApasatButonCaine = WinApiClass.CreateEvent(IntPtr.Zero, true, true, null);
            handleCatel = WinApiClass.CreateThread(IntPtr.Zero, 0, threadCatel, IntPtr.Zero, 0, out firCatel);
        }
    }
}
