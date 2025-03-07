﻿using Corel.Interop.VGCore;
using IS_Studio_Corel;
using IS_Studio_Corel.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Documents;
using Page = Corel.Interop.VGCore.Page;

namespace ISStudioCorel
{
    public partial class UtilitariosCorel
    {
        private Corel.Interop.VGCore.Application corelApp;

        private List<ProdutoNavalhaMateus> ProdutosNavalhasMateus = new List<ProdutoNavalhaMateus>
            {
                new ProdutoNavalhaMateus("Aco19", "Aço 19mm", "cm",1, 0.55),
                new ProdutoNavalhaMateus("Aco19Laser", "aço 19mm Laser", "cm",1, 1.10),
                new ProdutoNavalhaMateus("Aco32", "Aço 32mm", "cm", 1,0.70),
                new ProdutoNavalhaMateus("Aco32Laser", "Aço 32mm Laser", "cm",1, 1.40),
                new ProdutoNavalhaMateus("NavalhaGrafica", "Navalha Gráfica", "cm",1, 0.60),
                new ProdutoNavalhaMateus("NavalhaFerramenta", "Navalha Ferramenta", "cm",1, 0.30),
                new ProdutoNavalhaMateus("Pinos_Mola", "Pino retrátil de 13mm", "unidade", 1, 6.0),
                new ProdutoNavalhaMateus("Arrombador_1mm", "Arrombador de 1mm", "cm",1, 1.50),
                new ProdutoNavalhaMateus("Pino_1mm", "Arrombador de 1mm", "cm",1, 1.50),
                new ProdutoNavalhaMateus("Vazador_1mm", "Vazador de 1 mm", "unidade",1, 1.5),
                new ProdutoNavalhaMateus("Vazador_1.5mm", "Vazador de 1,5 mm", "unidade",1, 2.0),
                new ProdutoNavalhaMateus("Vazador_2mm", "Vazador de 2 mm", "unidade", 1,3.5),
                new ProdutoNavalhaMateus("Vazador_2.5mm", "Vazador de 2,5 mm", "unidade",1, 4.0),
                new ProdutoNavalhaMateus("Vazador_3mm", "Vazador de 3 mm", "unidade",1, 4.0),
                new ProdutoNavalhaMateus("Vazador_3.5mm", "Vazador de 3,5 mm", "unidade",1, 4.0),
            };
        private List<Objetos> Objetos = new List<Objetos> { };

        private void ModificarTemporariamenteCultura()
        {
            CultureInfo culturaOriginal = CultureInfo.CurrentCulture;
            CultureInfo novaCultura = (CultureInfo)culturaOriginal.Clone();
            novaCultura.NumberFormat.NumberDecimalSeparator = ",";
            novaCultura.NumberFormat.NumberGroupSeparator = ".";
            CultureInfo.CurrentCulture = novaCultura;

            // escreva o codigo aqui

            CultureInfo.CurrentCulture = culturaOriginal;

        }
        
        public UtilitariosCorel()
        {
            CultureInfo.CurrentCulture = new CultureInfo("pt-BR");
            CultureInfo.CurrentUICulture = new CultureInfo("pt-BR");
           // this.corelApp = new Corel.Interop.VGCore.Application();
        }

        public ObservableCollection<VectorColor> GetVectorsColors()
        {

            if (!DocExists()) { return new ObservableCollection<VectorColor>(); }

            ObservableCollection<VectorColor> list = new ObservableCollection<VectorColor> { };
            Document doc = corelApp.ActiveDocument;
            ShapeRange sr = corelApp.ActiveSelectionRange;
            int idx = -1;
            string name;
            string description;
            string color;

            foreach (Shape s in sr)
            {
                idx = idx + 1;
                color = NormalizeColors(s);
                //name = s.Fill.UniformColor.Name;
                name = s.Outline.Color.Name;
                description = $" desc.: {name}";
                list.Add(new VectorColor(idx, name, description, color));
            }

            return list;
        }

        // Implementação do Bubble Sort como método estático
        public static void BubbleSort(double[] arr)
        {
            int n = arr.Length;
            bool swapped;

            do
            {
                swapped = false;
                for (int i = 1; i < n; i++)
                {
                    if (arr[i - 1] > arr[i])
                    {
                        // Troca os elementos
                        double temp = arr[i];
                        arr[i] = arr[i - 1];
                        arr[i - 1] = temp;
                        swapped = true;
                    }
                }
                n--;
            } while (swapped);
        }

        public void CreateDoc(double widthSize = 1200, double heightSize = 900, string name = "Temp")
        {
            ///<summary>
            ///Cria um documento com as medidas dadas
            ///</summary>
            ///<param widthSize>Largura do documento</param>
            ///<param heightSize>Altura do documento</param>
            ///<param name>Nome que será atribuido ao documento</param>
            ///<returns>Sem retorno</returns>

            Document doc = corelApp.CreateDocument();
            doc.Unit = cdrUnit.cdrMillimeter;
            doc.ReferencePoint = cdrReferencePoint.cdrCenter;
            doc.ActivePage.SetSize(widthSize, heightSize);
            if (name == null || name == "")
            {
                doc.Name = $"Temp_{corelApp.Documents.Count}";
            }
            else
            {
                doc.Name = name;
            }
        }

        public void CreateLayer(string nameLayer= "Layer created from a method")
        {
            ///Sumary
            /// Esse método cria ou ativa uma camada com o nome passado pelo parametro
            /// se a camada não existir na pagina , o metodo a cria
            /// Se a camada já existir na pagina, o metodo apenas a torna ativa.
            ///

            Document doc = corelApp.Application.ActiveDocument;
            Page pag = doc.ActivePage;
            bool isLayer = false;

            foreach (Layer lyr in pag.Layers)
            {
                if(lyr.Name.Equals(nameLayer)) {
                    isLayer = true;
                    lyr.Activate();
                    break;
                }
            }

            if (!isLayer)
            {
                pag.CreateLayer(nameLayer);
            }

        }

        public void CreateCardWithTheBudgetRazors(string curveLenght)
        {
            if (!DocExists()) return;
            Document doc = corelApp.Application.ActiveDocument;
            Page pag = doc.ActivePage;
            Layer lyr = pag.ActiveLayer;
            Shape s;

            doc.Unit = cdrUnit.cdrMillimeter;
            doc.ReferencePoint = cdrReferencePoint.cdrCenter;
            doc.DrawingOriginX = pag.SizeWidth / 2;
            doc.DrawingOriginY = -(pag.SizeHeight / 2);

            bool success;

            double squareWidth = 130;
            double squareHeight = 50;
            double borderDistance = 10;
            string layerName = "Orçamento Navalhas Mateus";
            string borderName = "Borda Datasheet";
            //string lenghtRazorsName = "Medida das Navalhas";
            string valueRazorsName = "Valor da Navalhas";
            string valuePinosMolaName = "Valor dos ";
            double valuePerMm = 0.60;
            string fontName = "Arial";
            float fontSize = 20;


            // Cria uma nova lista com os nomes e quantidades dos objetos da seleção
            // que que conicidem como os objetos na lista de objetos da navalha do Mateus
            ShapeRange sr = corelApp.ActiveSelectionRange;
            List<ProdutoNavalhaMateus> budgetObjects = new List<ProdutoNavalhaMateus> { };
            List<CorelShape> listCorelShapes = new List<CorelShape> { };
            foreach (Shape s1 in sr)
            {
                CorelShape obj = new CorelShape()
                {
                    Id = s1.StaticID,
                    Name = s1.Name,
                    CurveLength = s1.Curve.Length,
                    Quantity = 1,
                    Color = s1.Outline.Color.ToString(),
                };

                listCorelShapes.Add(obj);
            }

            //budgetObjects = listCorelShapes.Where(p1 => ProdutosNavalhasMateus.Any(p2 => p2.Name == p1.Name && p2.Id == p1.Id)).ToList();

            //foreach (Shape s1 in sr)
            //{
            //    foreach (var item in ProdutosNavalhasMateus)
            //    {
            //        string msg = $"{s1.Name.ToString()} == {item.Name}";
            //        MessageBox.Show(msg);


            //        // trataObjeto() -> converter em curva etc.
            //        if (item.Name == s1.Name.ToString())
            //        {
            //            if (item.Unit == "unidade" || item.Unit == "Unidade")
            //            {
            //                budgetObjects.Add(item.Quantity = 1);
            //            }
            //            else
            //            {
            //                item.Quantity = s1.Curve.Length;
            //            }
            //            break;
            //        }
            //        else
            //        {
            //            //item.Name = "Não Identificado";
            //            item.Quantity = s1.Curve.Length;
            //        }

            //        budgetObjects.Add(item);
            //    }
            //}

            //Saida para controle visual
            string str = "";
            foreach (CorelShape obj in listCorelShapes)
            {
                str += $"{obj.Id} - {obj.Name} - {obj.Quantity} - {obj.CurveLength} \n";
            }
            MessageBox.Show(str);

            CreateLayer(layerName);

            // Cria Borda do datasheet
            DeleteShapesWithTheName(borderName);
            s = pag.ActiveLayer.CreateRectangle2(-squareWidth, 0, squareWidth, squareHeight);
            s.Outline.Width = 1.5;
            s.Outline.Color.CMYKAssign(0, 0, 0, 100);
            s.Name = borderName;

            // Cria texto para o Titulo
            DeleteShapesWithTheName(layerName);
            s = pag.ActiveLayer.CreateArtisticText(0, 0, layerName,
                                                    cdrTextLanguage.cdrBrazilianPortuguese,
                                                    cdrTextCharSet.cdrCharSetMixed, fontName, fontSize,
                                                    cdrTriState.cdrTrue, cdrTriState.cdrFalse, cdrFontLine.cdrSingleThinFontLine);
            s.Name = layerName;
            s.Fill.UniformColor.CMYKAssign(0, 100, 100, 0);
            s.SetPosition((-squareWidth + s.SizeWidth / 2) + borderDistance, squareHeight - borderDistance);

            
            // Cria texto para o valor das navalhas
            DeleteShapesWithTheName(valueRazorsName);
            double valueOfRazors = 0.0;
            string strValueOfRazors;

            // Captura o comprimento das navalhas
            success = double.TryParse(curveLenght, out valueOfRazors);

            if (success)
            {
                valueOfRazors = valueOfRazors * valuePerMm;
                strValueOfRazors = $"{curveLenght} mm x {valuePerMm} = R$ {valueOfRazors.ToString("F2")}";
            }
            else
            {
                //valueOfRazors = 0;
                strValueOfRazors = "Não foi possível obter um valor";
            };
            s = pag.ActiveLayer.CreateArtisticText(0, 0, strValueOfRazors,
                                                    cdrTextLanguage.cdrBrazilianPortuguese,
                                                    cdrTextCharSet.cdrCharSetMixed, fontName, fontSize,
                                                    cdrTriState.cdrTrue);
            s.Name = valueRazorsName;
            s.Fill.UniformColor.CMYKAssign(0, 100, 100, 0);
            s.SetPosition((-squareWidth + s.SizeWidth / 2) + borderDistance, squareHeight - ((borderDistance + s.SizeHeight) + 5));


            // Cria texto para cada valor da Lista
            double space=0;
            List<string> listaDeNomes = new List<string> { "Pino-Mola", "Vazador_1mm", "Vazador_1.5mm", "Vazador_2mm" };
            foreach (var produto in ProdutosNavalhasMateus)
            {
                space += 15;
                DeleteShapesWithTheName(valuePinosMolaName+produto.Name);
                double quantOfElement = CountShapesWithName(produto.Name);
                double valueOfElement;
                string strValueOfPinosMola;

                valueOfElement = quantOfElement * produto.Value;
                strValueOfPinosMola = $"{quantOfElement} x {produto.Value.ToString("F2")} = R$ {valueOfElement.ToString("F2")}";

                s = pag.ActiveLayer.CreateArtisticText(0, 0, strValueOfPinosMola,
                                                        cdrTextLanguage.cdrBrazilianPortuguese,
                                                        cdrTextCharSet.cdrCharSetMixed, fontName, fontSize,
                                                        cdrTriState.cdrTrue);
                s.Name = produto.Name;
                s.Fill.UniformColor.CMYKAssign(0, 100, 100, 0);
                s.SetPosition((-squareWidth + s.SizeWidth / 2) + borderDistance, squareHeight - ((borderDistance + s.SizeHeight) + space));
             }

            // Reposiciona as reguas
            doc.DrawingOriginX = -pag.SizeWidth / 2;
        }

        public void CreateScaleWithValues(int numPilot, int numDownProgress, int numUpperProgress, double idWidth, double idHeight)
        {

            if (!SelectExists()) { return; }

            Document doc = corelApp.ActiveDocument;
            doc.Unit = cdrUnit.cdrMillimeter;
            doc.ReferencePoint = cdrReferencePoint.cdrCenter;
            ShapeRange sr = doc.SelectionRange;
            ShapeRange srScales = new ShapeRange();
            Shape s;
            double dist = 20;
            int numScale = 0;

            if (sr.Count > 1)
            {
                MessageBox.Show("Select just one shape to scale", "IS Studio", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                sr[1].Name = $"Pilot_{numPilot}";

                numScale = numPilot;
                for (int i = 1; i < numDownProgress + 1; i++)
                {
                    s = sr[1].Duplicate();
                    s.SetSize(s.SizeWidth + (idWidth * i), s.SizeHeight + (idHeight * i));
                    //s.SetPosition(((s.PositionX - (s.SizeWidth*i)) - (dist + idWidth*i)) , s.PositionY + (idHeight * i));
                    s.Name = $"scale_{numScale += 1}";
                    srScales.Add(s);
                }

                numScale = numPilot;
                for (int i = 1; i < numUpperProgress + 1; i++)
                {
                    s = sr[1].Duplicate();
                    s.SetSize(s.SizeWidth - (idWidth * i), s.SizeHeight - (idHeight * i));
                    //s.SetPosition(((s.PositionX + (s.SizeWidth * i)) + (dist + idWidth*i)), s.PositionY - (idHeight * i));
                    s.Name = $"scale_{numScale -= 1}";
                    srScales.Add(s);
                }

                //OrganizeVectorsBySize(srScales);
                double desloc = 0;
                double deslocAcum = 0;
                int flag = 1;
                foreach (Shape s1 in srScales)
                {
                    desloc = s1.SizeWidth + dist;
                    if (flag == 1)
                        deslocAcum = desloc;
                    s1.PositionX = deslocAcum;
                    deslocAcum += desloc;
                    flag += 1;
                }

            }
        }
       
        public void CreateColorReference(string str, int cardLocation)
        {
            Document doc = corelApp.ActiveDocument;
            doc.Unit = cdrUnit.cdrMillimeter;
            doc.ReferencePoint = cdrReferencePoint.cdrCenter;
            Page pag = doc.ActivePage;
            Layer lyr;
            Shape s;
            double posX=0;
            double PosY=0;

            try
            {
                // procura pelo card adequado e Define posicao do texto
                CreateLayer($"Card_{cardLocation}");
                lyr = pag.ActiveLayer;

                switch (cardLocation)
                {
                    case 1:
                        posX = 68;
                        PosY = 100.5;
                        break;
                    case 2:
                        posX = 203;
                        PosY = 100.5;
                        break;
                    case 3:
                        posX = 65;
                        PosY = 10.5;
                        break;
                    case 4:
                        posX = 203;
                        PosY = 10.5;
                        break;
                    default:
                        posX = 0;
                        PosY = 0;
                        break;
                }

                s = lyr.CreateArtisticText(posX, PosY, str,
                        cdrTextLanguage.cdrBrazilianPortuguese, cdrTextCharSet.cdrCharSetMixed,
                        "Arial", 12, cdrTriState.cdrTrue);
                s.Name = $"Card_{cardLocation}_Color Reference";
                s.PositionX -= (s.SizeWidth / 2);
                s.PositionY -= (s.SizeHeight / 2);
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Exceção Inesperada\n{ex.Message}");
            }

        }

        public void CreateLabelText(string str,string strLayerName, int position)
        {
            Document doc = corelApp.ActiveDocument;
            doc.Unit = cdrUnit.cdrMillimeter;
            doc.ReferencePoint = cdrReferencePoint.cdrCenter;
            Page pag = doc.ActivePage;
            Layer lyr;
            Shape s;
            double posX = 0;
            double PosY = 0;
            string name=string.Empty;

            try
            {
                CreateLayer(strLayerName);
                lyr = pag.ActiveLayer;

                switch (position)
            {
                case 1: // Name
                    posX = 25;
                    PosY = 200;
                    name = "Name";
                    break;
                case 2: // Reference
                    posX = 20;
                    PosY = 190;
                    name = "Reference";
                    break;
                case 3: // Size
                    posX = 26;
                    PosY = 180;
                    name = "Size";
                    break;

                default:
                    posX = 0;
                    PosY = 0;
                    break;
            }
            
                s = lyr.CreateArtisticText(posX, PosY, str,
                        cdrTextLanguage.cdrBrazilianPortuguese, cdrTextCharSet.cdrCharSetMixed,
                        "Arial", 24, cdrTriState.cdrTrue);
                s.PositionX -= (s.SizeWidth / 2);
                s.PositionY -= (s.SizeHeight / 2);
                s.Name = $"Label_{name}";
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Exceção Inesperada\n{ex.Message}");
            }

        }

        public double CountShapesWithName(string shapeName)
        {
            try
            {
                Document doc = corelApp.ActiveDocument;
                doc.Unit = cdrUnit.cdrMillimeter;
                doc.ReferencePoint = cdrReferencePoint.cdrCenter;
                Page pag = doc.ActivePage;
                ShapeRange sr = doc.SelectionRange;

                // Troca Texto
                //Shape s = corelApp.ActiveSelectionRange.Shapes.FindShape("|Label|");
                //s.Text.Story.Text = "New Text";

                string query = $"@name ='Pino-Mola'";

                //ShapeRange srSelec = doc.ActivePage.Shapes.FindShapes("", 0, false, query);
                //doc.ActivePage.Shapes.FindShapes(Query: "@name.find('Pino-Mola')").CreateSelection();
                ShapeRange srSelec = doc.ActivePage.Shapes.FindShapes(Query: query);
                //srSelec.AddToSelection();

                return srSelec.Count;
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Execeção não prevista:\n\n {ex}");
                return 0;
            }
        }

        public void DeleteImagesInLayer(string nomeDaCamada)
        {
            if (!DocExists()) { return; }

            Document doc = corelApp.ActiveDocument;
            doc.Unit = cdrUnit.cdrMillimeter;
            doc.ReferencePoint = cdrReferencePoint.cdrCenter;
            Page pag = doc.ActivePage;
            try
            {
                Layer lyr = pag.Layers[nomeDaCamada];
                ShapeRange sr = lyr.SelectableShapes.All();

                foreach (Shape s in sr)
                {
                    if (s.Type == cdrShapeType.cdrBitmapShape) { s.Delete(); }
                } 
            }
            catch (Exception)
            {

                MessageBox.Show("Não foi possivel acessar a camada");
            }

        }

        public void DeleteShapesWithTheName(string nameShape) {
            Document doc = corelApp.Application.ActiveDocument;
            Page pag = doc.ActivePage;
            ShapeRange sr;

            sr = pag.FindShapes(nameShape);
            foreach (Shape s1 in sr)
            {
                s1.Delete();
            }
        }

        public void DeleteAllEmptyLayers()
        {
            Document doc = corelApp.Application.ActiveDocument;
            Page pag = doc.ActivePage;
            Layers allLayers = pag.Layers;

            //MessageBox.Show($"Foram detectadas {allLayers.Count} camadas");
            foreach (Layer lyr in allLayers)
            {
                if (!lyr.IsSpecialLayer)
                { 
                    ShapeRange sr = lyr.Shapes.All();
                    //MessageBox.Show($"A camada {lyr.Name} possui {sr.Count} shapes");
                    if (sr.Count == 0)
                        lyr.Delete();
                }
            }
        }

        public void DeselectVectorsWithColor(string color)
        {
            try
            {
                Document doc = corelApp.ActiveDocument;
                doc.Unit = cdrUnit.cdrMillimeter;
                doc.ReferencePoint = cdrReferencePoint.cdrCenter;
                Page pag = doc.ActivePage;
                ShapeRange sr = doc.SelectionRange;

                Color c = new Color();
                c.HexValue = color;
                c.ConvertToRGB();
                string query = $"@fill.color = rgb({c.RGBRed}, {c.RGBGreen}, {c.RGBBlue})" +
                    $"or @outline.color = rgb({c.RGBRed}, {c.RGBGreen}, {c.RGBBlue})";

                ShapeRange srSelec = doc.ActivePage.Shapes.FindShapes("", 0, false, query);
                //ShapeRange srSelec = doc.ActivePage.Shapes.FindShapes(Query: query);
                srSelec.RemoveFromSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Execeção não prevista:\n\n {ex}");
            }
        }

        public bool DocExists()
        {
            int Ndocs = corelApp.Documents.Count;
            if (Ndocs > 0)
            {
                return true;
            }
            else
            {
                MessageBox.Show("Nenhum documento para trabalhar", "IS Studio",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

        }

        public double GetRazorsLength()
        {
            if (!DocExists()) { return 0; }

            Document doc = corelApp.ActiveDocument;
            doc.Unit = cdrUnit.cdrMillimeter;
            doc.ReferencePoint = cdrReferencePoint.cdrCenter;
            Page pag = doc.ActivePage;
            ShapeRange sr = doc.SelectionRange.UngroupAllEx();
            double comprimento = 0;

            List<string> listaDescarte = new List<string> { "Pino-Mola", "Vazador_1mm", "Vazador_1.5mm", "Vazador_2mm"};

            if (sr != null)
            {
                foreach (Shape s in sr)
                {
                    if (s.Type != cdrShapeType.cdrCurveShape)
                    {
                        s.ConvertToCurves();
                        if (s.Type == cdrShapeType.cdrCurveShape)
                        {
                            comprimento = comprimento + s.Curve.Length;
                        }
                        else
                        {
                            s.Name = $" {s.StaticID} Não convertido em curvas";
                            MessageBox.Show($"Não foi possível converter o objeto {s.StaticID} em curvas", "IS Studio",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                            comprimento = comprimento + 0;
                        }
                    }
                    else
                    {
                        bool flag = false;
                        foreach (string name in listaDescarte)
                        {
                            // Condicional que verifica se o valor está na lista
                            if (s.Name == name)
                            {
                                flag = true;
                                MessageBox.Show($"Encontrei {s.Name}");
                                break;
                            }
                        }
                        if (flag == false)
                        {
                            // Se o valor não estiver na lista, ele é somado
                            comprimento = comprimento + s.Curve.Length;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Nenhum vetor está selecionado", "IS Studio",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return comprimento;
        }

        public double GetVectorsLength()
        {
            if (!DocExists()) { return 0; }

            Document doc = corelApp.ActiveDocument;
            doc.Unit = cdrUnit.cdrMillimeter;
            doc.ReferencePoint = cdrReferencePoint.cdrCenter;
            Page pag = doc.ActivePage;
            Layer tempLyr;
            ShapeRange srCopy;
            ShapeRange sr = doc.SelectionRange.UngroupAllEx();
            double comprimento = 0;


            if (sr != null)
            {
                tempLyr = pag.CreateLayer("tempLenght");
                srCopy = sr.Duplicate();
                srCopy.MoveToLayer(tempLyr);

                foreach (Shape s in srCopy)
                {
                    if (s.Type != cdrShapeType.cdrCurveShape)
                    {
                        s.ConvertToCurves();
                        if (s.Type == cdrShapeType.cdrCurveShape)
                        {
                            comprimento = comprimento + s.Curve.Length;
                        }
                        else
                        {
                            s.Name = $" {s.StaticID} Não convertido em curvas";
                            MessageBox.Show($"Não foi possível converter o objeto {s.StaticID} em curvas", "IS Studio",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                            comprimento = comprimento + 0;
                        }
                    }
                    else
                    {
                        comprimento = comprimento + s.Curve.Length;
                    }
                }

                // remover camada temporia e todo seu conteudo
                tempLyr.Delete();
            }
            else
            {
                MessageBox.Show("Nenhum vetor está selecionado", "IS Studio",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return comprimento;
        }

        public double GetVetorsAssemblyArea()
        {
            if (!DocExists()) { return 0; }

            Document doc = corelApp.ActiveDocument;
            doc.Unit = cdrUnit.cdrMillimeter;
            ShapeRange sr = doc.SelectionRange;
            double area = 0;

            if (sr != null)
            {
                area = sr.SizeWidth * sr.SizeHeight;
            }
            else
            {
                MessageBox.Show("Nenhum vetor está selecionado", "IS Studio",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return area;
        }

        public double GetSizeWidth()
        {

            if (!DocExists()) { return 0; }

            Document doc = corelApp.ActiveDocument;
            doc.Unit = cdrUnit.cdrMillimeter;
            ShapeRange sr = doc.SelectionRange;
            double width = 0;
            if (sr.Count != 1)
            {
                MessageBox.Show($"Select a shape");
            }
            else
            {
                width = sr[1].SizeWidth;
            }
            return width;
        }

        public double GetSizeHeight()
        {
            if (!DocExists()) { return 0; }

            Document doc = corelApp.ActiveDocument;
            doc.Unit = cdrUnit.cdrMillimeter;
            ShapeRange sr = doc.SelectionRange;
            double height = 0;
            if (sr.Count != 1)
            {
                MessageBox.Show($"Select a shape");
            }
            else
            {
                height = sr[1].SizeHeight;
            }
            return height;
        }

        public double GetValueForProgressionFactor(string direction = "width")
        {
            double factorValue = 0;

            if (!SelectExists()) { return factorValue; }

            Document doc = corelApp.ActiveDocument;
            doc.Unit = cdrUnit.cdrMillimeter;
            ShapeRange sr = doc.SelectionRange;
            double[] lengths = new double[sr.Count];
            List<double> progressionsList = new List<double>();

            if (sr.Count <= 1)
            {
                MessageBox.Show("Selecione pelo menos duas formas para obter o fator de escala", "IS Studio", MessageBoxButton.OK, MessageBoxImage.Warning);
                return factorValue;
            }

            int i = 0;
            if (direction == "width")
            {
                foreach (Shape s in sr)
                {
                    lengths[i] = s.SizeWidth;
                    i++;
                }
            }
            else if (direction == "height")
            {
                foreach (Shape s in sr)
                {
                    lengths[i] = s.SizeHeight;
                    i++;
                }
            }

            //ordena o array
            BubbleSort(lengths);

            double prog = 0;
            for (i = lengths.Length - 1; i > 0; i--)
            {
                if (i <= 0) { break; }
                prog = lengths[i] - lengths[i - 1];
                progressionsList.Add(prog);
            }

            factorValue = progressionsList.Average();
            return factorValue;
        }

        public List<double> GetListProgressionsFactors(string direction="width")
        {

            if (!DocExists()) { return null; }

            Document doc = corelApp.ActiveDocument;
            doc.Unit = cdrUnit.cdrMillimeter;
            ShapeRange sr = doc.SelectionRange;
            double[] lengths = new double[sr.Count];
            List<double> progressionsFactors = new List<double>();

            if (sr.Count <= 1)
            {
                MessageBox.Show("Selecione pelo menos duas formas para obter o fator de escala", "IS Studio", MessageBoxButton.OK, MessageBoxImage.Warning);
                return progressionsFactors;
            }

            //cria array com os tamanhos
            int i = 0;
            if (direction == "width")
            {
                foreach (Shape s in sr)
                {
                    lengths[i] = s.SizeWidth;
                    i++;
                }
            }
            else if (direction == "height")
            {
                foreach (Shape s in sr)
                {
                    lengths[i] = s.SizeHeight;
                    i++;
                }
            }

            //ordena o array
            BubbleSort(lengths);

            //converte o array para uma lista
            double prog = 0;
            for (i = lengths.Length - 1; i > 0; i--)
            {
                if (i <= 0) { break; }
                prog = lengths[i] - lengths[i - 1];
                progressionsFactors.Add(prog);
            }

            return progressionsFactors;
        }

        public string GetImageFilePath()
        {
            string imagePath = null;

            // Cria uma janela para abrir um arquivo
            var abrirArquivo = new OpenFileDialog();
            abrirArquivo.Title = "Selecione um arquivo de imagem";
            abrirArquivo.Filter = "Arquivos de imagem|*.jpg;*.png";
            abrirArquivo.RestoreDirectory = true;

            // Pega o resultado de qual botão ele clicou
            var resultado = abrirArquivo.ShowDialog();

            // User escolheu um arquivo
            if (resultado == true)
            {
                imagePath = abrirArquivo.FileName;
                // Abre a imagem em um PictureBox ou similar
                //pictureBox1.Image = Image.FromFile(abrirArquivo.FileName);
            }
            else
            {
                // Refatorar para Obter o caminho do arquivo dinamicamente
                imagePath = "C:\\Program Files\\Corel\\CorelDRAW Graphics Suite 2022\\Programs64\\Addons\\IS_Studio_PlanetShoes\\Imgs\\no - image.jpg";
            }

            return imagePath;
        }

        public string GetStringPart(string strText, char separator, short part, bool reverse)
        {
            /// SUMARY
            /// Esse metodo retona um sub string obtido através da divisão do string principal
            /// na presença de um carcter especial '_'
            /// 
            string splitPart = "";
            part -= 1;
            try
            {
                string[] parts = strText.Split(separator);

                if (parts.Length == 0)
                {
                    splitPart = "No Name";
                    return splitPart;
                }

                if (reverse == true)
                    Array.Reverse(parts);

                splitPart = parts[part];

                return splitPart;

            }
            catch (Exception)
            {
                MessageBox.Show($"Unable to get text\n\nThe shape name must have at least one underscore '_'", "IS Studio", MessageBoxButton.OK, MessageBoxImage.Warning);
                return splitPart;
            }

        }

        public double GetLenghtCurve()
        {
            double curveLenght = 0;
            if (!DocExists()) { return curveLenght; }

            Document doc = corelApp.ActiveDocument;
            doc.Unit = cdrUnit.cdrMillimeter;
            doc.ReferencePoint = cdrReferencePoint.cdrCenter;
            ShapeRange sr = doc.SelectionRange;
            ShapeRange srCopy;
            short isCurve = 0;
            string strQuantNoCrv=null;

            if (!SelectExists()) { return curveLenght; }

            srCopy = sr.Duplicate().UngroupAllEx(); ;
            foreach (Shape s in srCopy)
            {
                // recolhe e formata a saida das formas que nao podem ser convertidas em curvas
                try
                {
                    // Lida com objetos com contorno
                    //if (s.Type == cdrShapeType.cdrContourGroupShape)
                    //{
                    //    ShapeRange srUngroup = s.Duplicate().BreakApartEx();
                    //    srUngroup[1].Selected = true;
                    //    srUngroup[1].ConvertToCurves();
                    //    curveLenght += srUngroup[1].Curve.Length;
                    //    srUngroup.Delete();
                    //}
                    s.ConvertToCurves();
                    curveLenght += s.Curve.Length;
                }
                catch (Exception)
                {
                    if (isCurve < 10)
                    {
                        strQuantNoCrv = string.Concat(strQuantNoCrv, $"{s.StaticID} - ");
                        isCurve = 0;
                    }
                    else
                    {
                        strQuantNoCrv = string.Concat(strQuantNoCrv, $"{s.StaticID}\n");
                    }
                    isCurve += 1;
                }
            }
            srCopy.Delete();

            if (isCurve > 0)
            {
                MessageBox.Show($"It was not possible to turn some forms into curves:\n {strQuantNoCrv}", "IS Studio", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            return curveLenght;
        }

        public List<String> GetAllLayers()
        {
            List<String> ListOfLayers = new List<String> { };

            if (!DocExists()) { return ListOfLayers; }

            Document doc = corelApp.ActiveDocument;
            doc.Unit = cdrUnit.cdrMillimeter;
            doc.ReferencePoint = cdrReferencePoint.cdrCenter;
            Page pag = doc.ActivePage;

            foreach (Layer lyr in pag.Layers)
            {
                if (!lyr.IsSpecialLayer)
                    ListOfLayers.Add (lyr.Name);
            }

            return ListOfLayers;
        }

        public void ImportImageToCatalogPage(string imagePath, int cardLocation)
        {
            if (!DocExists()) { return; }

            Document doc = corelApp.ActiveDocument;
            doc.Unit = cdrUnit.cdrMillimeter;
            doc.ReferencePoint = cdrReferencePoint.cdrCenter;
            Page pag = doc.ActivePage;
            Layer lyr;
            ShapeRange sr = doc.SelectionRange;

            double posX = 0, posY = 0;
            double cardWidth = 0;
            double cardHeight = 0;

            string lyrName = $"Card_{cardLocation + 1}";

            CreateLayer(lyrName);
            lyr = pag.Layers[lyrName];
            DeleteImagesInLayer(lyrName);

            try
            {
                lyr.Import(imagePath);
                Shape sImage = lyr.Shapes[1];

                // cria card
                switch (cardLocation + 1)
                {
                    case 1:
                        cardWidth = 125;
                        cardHeight = 80;
                        posX = 96;
                        posY = 145;
                        break;

                    case 2:
                        cardWidth = 125;
                        cardHeight = 80;
                        posX = 230;
                        posY = 145;
                        break;

                    case 3:
                        cardWidth = 125;
                        cardHeight = 80;
                        posX = 96;
                        posY = 55;
                        // no centro
                        //posX = 163;
                        //posY = 55;
                        break;

                    case 4:
                        // Localiza Card_3 e reposiciona
                        //cardWidth = 125;
                        //cardHeight = 80;
                        //posX = 96;
                        //posY = 55;

                        cardWidth = 125;
                        cardHeight = 80;
                        posX = 230;
                        posY = 55;
                        break;

                    case 5:
                        // Localiza Card_3 e reposiciona
                        //cardWidth = 84;
                        //cardHeight = 59;
                        //posX = 75;
                        //posY = 50;

                        // Localiza Card_4 e reposiciona
                        //cardWidth = 84;
                        //cardHeight = 59;
                        //posX = 163;
                        //posY = 50;

                        cardWidth = 84;
                        cardHeight = 59;
                        posX = 251;
                        posY = 50;
                        break;

                    case 6:
                        // Localiza Card_1, Card_2, Card_3, Card_4, Card_5 e reposiciona
                        posX = 0;
                        posY = 0;
                        break;

                    default:
                        break;
                }
                sImage.Name = $"{lyrName}";
                sImage.SetSize(cardWidth, cardHeight);
                sImage.SetPosition(posX, posY);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nenhuma imagem foi selecionada\n {ex.GetType()}");
                return;
            }


        }

        public void LoadCatalogPage()
        {
            //// Criando uma nova página
            //CatalogPage minhaPagina = new CatalogPage
            //{
            //    // Definindo o tamanho da página
            //    Tamanho = new CatalogPageSize { Largura = 21.0, Altura = 29.7 },

            //    // Definindo o label da página
            //    Label = new CatalogPageLabel { Nome = "Minha Página", Referencia = "Ref123", NumeracaoMinima = 1 },

            //    // Definindo o cabeçalho da página
            //    Cabecalho = new CatalogPageHeader { TextoCentral = "Título Central", TextoDireito = "Título Direito" },

            //    // Definindo os cards da página
            //    Cards = new List<CatalogPageCard>
            //    {
            //        new CatalogPageCard { Imagem = "caminho/para/imagem.jpg", ReferenciaCor = "#FFFFFF", Descricao = "Descrição do Card 1" },
            //        new CatalogPageCard { Imagem = "caminho/para/outra/imagem.jpg", ReferenciaCor = "#000000", Descricao = "Descrição do Card 2" }
            //    },

            //    // Definindo o rodapé da página
            //    Rodape = new CatalogPageFoot { TextoEsquerdo = "Texto Esquerdo", TextoCentral = "Texto Central", TextoDireito = "Texto Direito" }
            //};

            //txtName.Text = minhaPagina.Label.Nome;

            MessageBox.Show("Lendo uma pagina");
        }

        public void MoveToLayerWithTheShapeName(short positionOfText = 2, bool reverse = true)
        {
            if (!DocExists()) { return; }

            Document doc = corelApp.ActiveDocument;
            doc.Unit = cdrUnit.cdrMillimeter;
            doc.ReferencePoint = cdrReferencePoint.cdrCenter;
            Page pag = doc.ActivePage;

            ShapeRange sr = doc.SelectionRange;
            if (sr.Count == 0)
            {
                MessageBoxResult resp = MessageBox.Show("Nothing selected.\nDo you want to search the page?", "IS Studio", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resp == MessageBoxResult.Yes)
                {
                    sr = pag.Shapes.All();
                    sr.CreateSelection();
                }
                else
                {
                    MessageBox.Show("No shape was selected", "IS Studio", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            foreach (Shape s in sr)
            {
                string strCamada = GetStringPart(s.Name, '_', positionOfText, reverse);
                //MessageBox.Show($"Criando {strCamada}", "IS Studio");
                if (strCamada == null || strCamada == "") return;

                s.Layer.Activate();
                CreateLayer(strCamada);
                s.MoveToLayer(pag.ActiveLayer);
            }
            DeleteAllEmptyLayers();

            MessageBox.Show("Done!", "IS Studio", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        public string NormalizeColors(Shape s, string ValorHEXParaCorPadrao = "#929497")
        {
            /// <summary>
            /// Normaliza as cores de uma seleção com base em suas cores de contorno e preenchimento.
            /// Ela padroniza as cores invalidas das formas selecionadas
            /// Se a forma não possuir nem cor de contorno , nem cor de preenchimento, será retornado o valor
            /// padrao "#96989A", se existir uma cor de contorno, ela será retornada.
            /// Na ausêcia de cor de contorno será atribuida a cor de preenchimento se esse for do tipo Uniforme,
            /// do contrario será aplicada a cor padrão
            /// </summary>
            /// <param corelApp>Instancia do Aplicativo.</param>
            /// <param s>Forma valida do corelDraw.</param>
            /// <param CorPadrao>Valor Hexadecimal para a cor que substituira as cores invalidas do objetos.</param>
            /// <returns> O valor em Hexadecimal aplicado a forma. </returns>
            /// 

            string corHEX = "";
            string corPadrao = ValorHEXParaCorPadrao;

            //Verifica se o objeto não possui nem contorno nem preenchimento
            if (s.Fill.Type == cdrFillType.cdrNoFill && s.Outline.Type == cdrOutlineType.cdrNoOutline)
            {
                corHEX = corPadrao;
                s.Outline.Width = 0.01;
                s.Outline.Color.HexValue = corHEX; // aplica cinza medio
                return corHEX;
            }

            // Verifica se o objeto possui preenchimento uniforme
            if (s.Fill.Type == cdrFillType.cdrUniformFill)
            {
                corHEX = s.Fill.UniformColor.HexValue;
            }
            else // o objeto não possui preenchimento uniforme
            {

                // Verifica se o objeto não possui contorno
                if (s.Outline.Type == cdrOutlineType.cdrNoOutline)
                {
                    corHEX = corPadrao;
                }
                else // O objeto possui contorno
                {
                    corHEX = s.Outline.Color.HexValue;
                }
            }

            s.Fill.ApplyNoFill();
            s.Outline.Width = 0.01;
            s.Outline.Color.HexValue = corHEX;

            return corHEX;
        }

        public void OrganizeVectorsBySize(ShapeRange sr)
        {
            if (!SelectExists()) { return; }

            Document doc = corelApp.ActiveDocument;
            doc.Unit = cdrUnit.cdrMillimeter;

            //ShapeRange sr = doc.SelectionRange;

            double[] lengths = new double[sr.Count];
            List<double> progressionsList = new List<double>();

            int i = 0;
            foreach (Shape s in sr)
            {
                lengths[i] = s.SizeHeight;
                i++;
            }

            //ordena o array
            BubbleSort(lengths);

            double prog = 0;
            for (i = lengths.Length - 1; i > 0; i--)
            {
                if (i <= 0) { break; }
                prog = lengths[i] - lengths[i - 1];
                progressionsList.Add(prog);
            }


            string strArray = "";
            for (i = 0; i < lengths.Length; i++)
            {
                strArray = strArray + "- " + lengths[i].ToString("F3") + "\n";
            }

            string strList = "";
            foreach (var item in progressionsList)
            {
                strList = strList + "- " + item.ToString("F3") + "\n";
            }

            double media = progressionsList.Average();
            string msg = strArray + "\n" + strList + "\n" + media.ToString("F3");
            MessageBox.Show(msg);
        }

        public void PlacePinoMolaByClicking()
        {
            //The variable x returns the horizontal position of the mouse click.
            //The variable y returns the vertical position of the mouse click.
            //The parameter shift returns the combination of the Shift, Ctrl, and Alt keys that is held down by the user when clicking the mouse.
            //The Shift, Ctrl, and Alt keys are assigned values of 1, 2, and 4(respectively), the sum of which is the returned value.
            //The value 10 specifies the number of seconds for the user to click in the document.
            //The value True specifies that the SnapToObjects parameter is enabled.
            //The value cdrCursorPick specifies that the icon for the Pick tool is used for the cursor icon. (You cannot use a custom icon.)
            //One of the following values is returned:
            //0 — The user successfully completes the click.
            //1 — The user cancels by pressing Escape.
            //2 — The operation times out.

            if (!DocExists()) { return; }

            Document doc = corelApp.ActiveDocument;
            doc.Unit = cdrUnit.cdrMillimeter;
            doc.ReferencePoint = cdrReferencePoint.cdrCenter;
            Page pag = doc.ActivePage;

            double posX = 0, posY = 0;
            int shiftkey = 0;
            cdrCursorShape cursorShape = cdrCursorShape.cdrCursorPickOvertarget;
            int click = 0;

            while (click != 1 || click!=2) //press Scape Key
            {
                click = doc.GetUserClick(out posX, out posY, out shiftkey, 10, true, cursorShape);
                if (click !=1) {
                    CreateLayer("Pinos-Mola");

                    Shape s1 = pag.ActiveLayer.CreateEllipse2(posX, posY, 2.55, 2.55);
                    Shape s2 = pag.ActiveLayer.CreateEllipse2(posX, posY, 2.55 - 0.25, 2.55 - 0.25);
                    Shape s3 = pag.ActiveLayer.CreateEllipse2(posX, posY, 1, 1);

                    s2.AddToSelection();
                    s1.AddToSelection();
                    s1 = doc.Selection().Combine();
                    s1.Name = "Pino-Mola";
                    s1.Outline.SetNoOutline();

                    switch (shiftkey)
                    {
                        case 1://Shift Pressionado -> Ciano
                            s1.Fill.UniformColor.CMYKAssign(100, 0, 0, 0);
                            break;
                        case 2://Crtl Pressionado -> Magenta
                            s1.Fill.UniformColor.CMYKAssign(0, 100, 0, 0);
                            break;
                        case 4://Alt Pressionado -> Amarelo
                            s1.Fill.UniformColor.CMYKAssign(0, 0, 100, 0);
                            break;
                        default://Qualquer outra tecla
                            s1.Fill.UniformColor.CMYKAssign(100, 100, 100, 100);
                            break;
                    }
                }
                else if (click == 2) {break;}
                else if (click == 1) {break;}
            }
        }

        public void PlaceVazadorByClicking(double vazadorSize)
        {
            //The variable x returns the horizontal position of the mouse click.
            //The variable y returns the vertical position of the mouse click.
            //The parameter shift returns the combination of the Shift, Ctrl, and Alt keys that is held down by the user when clicking the mouse.
            //The Shift, Ctrl, and Alt keys are assigned values of 1, 2, and 4(respectively), the sum of which is the returned value.
            //The value 10 specifies the number of seconds for the user to click in the document.
            //The value True specifies that the SnapToObjects parameter is enabled.
            //The value cdrCursorPick specifies that the icon for the Pick tool is used for the cursor icon. (You cannot use a custom icon.)
            //One of the following values is returned:
            //0 — The user successfully completes the click.
            //1 — The user cancels by pressing Escape.
            //2 — The operation times out.


            //Dim s As Shape
            //Dim sp As SubPath
            //Dim crv As Curve
            //Set crv = CreateCurve(ActiveDocument)
            //ActiveDocument.ReferencePoint = cdrBottomLeft
            //Set sp = crv.CreateSubPath(1, 1)
            //sp.AppendLineSegment 1, 1
            //sp.AppendCurveSegment 3, 3
            //sp.AppendCurveSegment 5, 1
            //sp.AppendCurveSegment 7, 4
            //sp.AppendLineSegment 9, 0
            //sp.Nodes(2).Type = cdrSmoothNode
            //sp.Nodes(3).Type = cdrSmoothNode
            //sp.Nodes(4).Type = cdrSmoothNode
            //sp.Nodes(5).Type = cdrSmoothNode
            //Set s = ActiveLayer.CreateCurve(crv)


            if (!DocExists()) { return; }

            Document doc = corelApp.ActiveDocument;
            doc.Unit = cdrUnit.cdrMillimeter;
            doc.ReferencePoint = cdrReferencePoint.cdrCenter;
            Page pag = doc.ActivePage;

            double widthCurve = 0.1;
            double posX = 0, posY = 0;
            double tamX = ((vazadorSize / 2) - (widthCurve / 2));
            double tamY = ((vazadorSize / 2) - (widthCurve / 2));
            double dx = (Math.Cos(45) * tamX);
            double dy = (Math.Sin(45) * tamY);
            

            int shiftkey = 0;
            cdrCursorShape cursorShape = cdrCursorShape.cdrCursorPickOvertarget;
            int click = 0;

            while (click != 1 || click != 2) //press Scape Key
            {
                click = doc.GetUserClick(out posX, out posY, out shiftkey, 10, true, cursorShape);
                if (click != 1)
                {
                    CreateLayer($"Vazador_{vazadorSize}mm");

                    Shape s1 = pag.ActiveLayer.CreateEllipse2(posX, posY, tamX, tamY);
                    
                    SubPath sp1;
                    Curve crv1 = new Curve();
                    sp1 = crv1.CreateSubPath(posX, posY);
                    sp1.AppendLineSegment(posX - dx, posY + dy);
                    Shape s2 = pag.ActiveLayer.CreateCurve(crv1);

                    SubPath sp2;
                    Curve crv2 = new Curve();
                    sp2 = crv2.CreateSubPath(posX, posY);
                    sp2.AppendLineSegment(posX + dx, posY - dy);
                    Shape s3 = pag.ActiveLayer.CreateCurve(crv2);

                    SubPath sp3;
                    Curve crv3 = new Curve();
                    sp3 = crv3.CreateSubPath(posX, posY);
                    sp3.AppendLineSegment(posX + dx, posY + dy);
                    Shape s4 = pag.ActiveLayer.CreateCurve(crv3);

                    SubPath sp4;
                    Curve crv4 = new Curve();
                    sp4 = crv4.CreateSubPath(posX, posY);
                    sp4.AppendLineSegment(posX - dx, posY - dy);
                    Shape s5 = pag.ActiveLayer.CreateCurve(crv4);

                    s5.AddToSelection();
                    s4.AddToSelection();
                    s3.AddToSelection();
                    s2.AddToSelection();
                    s1.AddToSelection();
                    s1.Fill.ApplyNoFill();
                    s1.Outline.SetProperties(widthCurve);
                    s1 = doc.Selection().Combine();
                    s1.Outline.ConvertToObject();
                    s1.Name = $"Vazador_{vazadorSize}mm";

                    switch (shiftkey)
                    {
                        case 1://Shift Pressionado -> Ciano
                            s1.Fill.UniformColor.CMYKAssign(100, 0, 0, 0);
                            break;
                        case 2://Crtl Pressionado -> Magenta
                            s1.Fill.UniformColor.CMYKAssign(0, 100, 0, 0);
                            break;
                        case 4://Alt Pressionado -> Amarelo
                            s1.Fill.UniformColor.CMYKAssign(0, 0, 100, 0);
                            break;
                        default://Qualquer outra tecla
                            s1.Fill.UniformColor.CMYKAssign(100, 100, 100, 100);
                            break;
                    }
                }
                else if (click == 2) { break; }
                else if (click == 1) { break; }
            }
        }

        public void ReplaceVectorsWithSize(double sizeWidth, double sizeHeight, double toWidth, double toHeight)
        {
            try
            {
                if (!DocExists()) { return; }
                Document doc = corelApp.ActiveDocument;
                doc.Unit = cdrUnit.cdrMillimeter;
                doc.ReferencePoint = cdrReferencePoint.cdrCenter;

                // *************************************************************
                // Verificar a expressão da query para fazer o código funcionar
                // *************************************************************

                //double sW = Convert.ToDouble(sizeWidth);
                //double sH = Convert.ToDouble(sizeHeight);
                //double tW = Convert.ToDouble(toWidth);
                //double tH = Convert.ToDouble(toHeight);

                //ShapeRange sr = null;
                //string query = "@width = {" + sW + " mm} and @height = {" + sH + " mm}";
                //sr = doc.ActivePage.Shapes.FindShapes("", 0, false, query);

                //sr.AddToSelection();
                //foreach (Shape s in sr)
                //{
                //    s.SetSize(tW, tH);
                //}

                ShapeRange sr = doc.SelectionRange;
                ShapeRange sr1 = doc.ActivePage.Shapes.All();
                ShapeRange sr2 = null;

                foreach (Shape s in sr1)
                {
                    if (s.Type == cdrShapeType.cdrGroupShape)
                    {
                        // *************************************************
                        // Refatorar para fazer de forma recursiva para que
                        // a estrutura e grupos permaneça
                        // *************************************************

                        //Captura estrutura de nome e camada
                        string groupName = s.Name;
                        Layer lyr = s.Layer;

                        //Desagrupa para trabalhar

                        sr2 = s.UngroupAllEx();

                        //Trabalha
                        bool flagW = false;
                        bool flagH = false;
                        string shapeName = "Vazador";
                        foreach (Shape s2 in sr2)
                        {
                            if (s2.SizeWidth == sizeWidth)
                            {
                                s2.SizeWidth = toWidth;
                                flagW = true;
                            }

                            if (s2.SizeHeight == sizeHeight)
                            {
                                s2.SizeHeight = toHeight;
                                flagH = true;
                            }

                            // Nomeia objeto
                            if (flagW == true && flagH == true)
                            {
                                s2.Name = $"{shapeName}_{toWidth}x{toHeight}";
                            }
                        }

                        //Agrupa novamente
                        Shape s2group = sr2.Group();
                        s2group.Name = groupName;
                        s2group.MoveToLayer(lyr);
                    }
                    else
                    {
                        if (s.SizeWidth == sizeHeight)
                            s.SizeWidth = toWidth;

                        if (s.SizeHeight == sizeHeight)
                            s.SizeHeight = toHeight;
                    }


                }
                MessageBox.Show("Done!","IS Studio",MessageBoxButton.OK,MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unforeseen exception:\n\n {ex}");
            }
        }

        public void SendSelectionToLayer(string strCamada)
        {
            if (!DocExists()) { return; }
            if (!SelectExists()) { return; }

            Document doc = corelApp.ActiveDocument;
            doc.Unit = cdrUnit.cdrMillimeter;
            doc.ReferencePoint = cdrReferencePoint.cdrCenter;
            Page pag = doc.ActivePage;
            ShapeRange sr = doc.SelectionRange;
            Layer lyr;

            lyr = pag.Layers.Find(strCamada);

            foreach (Shape s in sr)
            {
                s.Layer.Activate();
                s.MoveToLayer(lyr);
            }

            MessageBox.Show($"Done!","IS Studio",MessageBoxButton.OK,MessageBoxImage.Information);
        }

        public bool SelectExists()
        {
            if (!DocExists()) { return false; }

            ShapeRange sr = corelApp.ActiveSelectionRange;
            if (sr.Count > 0)
            {
                return true;
            }
            else
            {
                MessageBox.Show("Nenhuma seleção encontrada.", "IS Studio",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

        }

        public void SelectVectorsWithColor(string color)
        {
            try
            {
                Document doc = corelApp.ActiveDocument;
                doc.Unit = cdrUnit.cdrMillimeter;
                doc.ReferencePoint = cdrReferencePoint.cdrCenter;
                Page pag = doc.ActivePage;
                ShapeRange sr = doc.SelectionRange;

                Color c = new Color();
                c.HexValue = color;
                c.ConvertToRGB();
                string query = $"@fill.color = rgb({c.RGBRed}, {c.RGBGreen}, {c.RGBBlue})" +
                    $"or @outline.color = rgb({c.RGBRed}, {c.RGBGreen}, {c.RGBBlue})";

                ShapeRange srSelec = doc.ActivePage.Shapes.FindShapes("", 0, false, query);
                //ShapeRange srSelec = doc.ActivePage.Shapes.FindShapes(Query: query);
                srSelec.AddToSelection();
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Execeção não prevista:\n\n {ex}");
            }
        }

        public void SelectShapesWithName(string shapeName)
        {
            
            try
            {
                Document doc = corelApp.ActiveDocument;
                doc.Unit = cdrUnit.cdrMillimeter;
                doc.ReferencePoint = cdrReferencePoint.cdrCenter;
                Page pag = doc.ActivePage;
                ShapeRange sr = doc.SelectionRange;

                // Troca Texto
                //Shape s = corelApp.ActiveSelectionRange.Shapes.FindShape("|Label|");
                //s.Text.Story.Text = "New Text";

                string query = $"@name ='Pino-Mola'";

                //ShapeRange srSelec = doc.ActivePage.Shapes.FindShapes("", 0, false, query);
                //doc.ActivePage.Shapes.FindShapes(Query: "@name.find('Pino-Mola')").CreateSelection();
                ShapeRange srSelec = doc.ActivePage.Shapes.FindShapes(Query: query);
                srSelec.AddToSelection();
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Execeção não prevista:\n\n {ex}");
            }
        }

        public void NameShapesWithParameters(string strPrefix, string strName, string strSuffix, char separator, string strEnum, string strInicio, string strPasso)
        {
            if (!DocExists()) { return; }
            if (!SelectExists()) { return; }

            try
            {
                Document doc = corelApp.ActiveDocument;
                doc.Unit = cdrUnit.cdrMillimeter;
                doc.ReferencePoint = cdrReferencePoint.cdrCenter;
                Page pag = doc.ActivePage;
                ShapeRange sr = doc.SelectionRange;
                double inicio;
                double passo;
                bool sucess;

                sucess = Double.TryParse(strInicio, out inicio);
                if (!sucess)
                    inicio = 0;

                sucess = Double.TryParse(strPasso, out passo);
                if (!sucess)
                    passo = 0;

                string shapeName = string.Empty;
                double numeracao = inicio;

                if (strPrefix == "" || strPrefix == "Prefix")
                {
                    strPrefix = string.Empty;
                }
                else
                {
                    strPrefix = strPrefix + separator;
                }

                if (strName == "" || strName == "Name")
                {
                    strName = string.Empty;
                }
                else
                {
                    strName = strName + separator;
                }

                if (strSuffix == "" || strSuffix == "Suffix")
                {
                    strSuffix = string.Empty;
                }
                else
                {
                    strSuffix = strSuffix + separator;
                }

                foreach (Shape s in sr)
                {
                    switch (strEnum)
                    {
                        case "auto":
                            shapeName = $"{strPrefix}{strName}{strSuffix}{numeracao.ToString()}";
                            numeracao = numeracao + passo;
                            break;
                        case "id":
                            numeracao = s.StaticID;
                            shapeName = $"{strPrefix}{strName}{strSuffix}{numeracao.ToString()}";
                            break;
                        case "sem":
                            numeracao = 0;
                            shapeName = $"{strPrefix}{strName}{strSuffix}";
                            break;
                    }
                    
                    s.Name = shapeName;
                }

                MessageBox.Show($"Done!", "IS Studio", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Execeção não prevista:\n\n {ex}");
            }
        }

        public void CreateFrameArroundTheSelection()
        {
            if (!DocExists()) { return; }
            if (!SelectExists()) { return; }

            try
            {
                Document doc = corelApp.ActiveDocument;
                doc.Unit = cdrUnit.cdrMillimeter;
                doc.ReferencePoint = cdrReferencePoint.cdrCenter;
                Page pag = doc.ActivePage;
                ShapeRange sr = doc.SelectionRange;
                ShapeRange srGroup =null;
                Shape sGroup;

                string lyrQuotaName = "Tecnica";
                string frameName = "Frame";
                string quotaName = "Cota";
                string measureName = "Measure";
                string txtQuota = string.Empty;

                double frameWidth = 0;
                double frameHeight = 0;
                double posX = 0;
                double posY = 0;
                double borderDistance = 10;
                string fontName = "Arial";
                float fontSize = 20;

                CreateLayer(lyrQuotaName);
                Layer lyrQuota = pag.ActiveLayer;

                foreach (Shape s in sr)
                {
                   
                    s.GetBoundingBox(out posX, out posY, out frameWidth, out frameHeight);

                    // Cria Borda para cota
                    Shape frame = pag.ActiveLayer.CreateRectangle2(-frameWidth, 0, frameWidth, frameHeight);
                    frame.Outline.Width = 1.5;
                    frame.Outline.Color.CMYKAssign(100, 0, 100, 0);
                    frame.Fill.UniformColor.CMYKAssign(100, 0, 100, 0);
                    frame.SetBoundingBox(posX,posY,frameWidth,frameHeight);
                    frame.Name = frameName + "_" + s.Name;
                    frame.MoveToLayer(lyrQuota);

                    // Cria texto para a cota
                    txtQuota = frameWidth.ToString("F2") + " x " + frameHeight.ToString("F2");
                    Shape cota = pag.ActiveLayer.CreateArtisticText(0, 0, txtQuota,
                                                            cdrTextLanguage.cdrBrazilianPortuguese,
                                                            cdrTextCharSet.cdrCharSetMixed, fontName, fontSize,
                                                            cdrTriState.cdrTrue, cdrTriState.cdrFalse);
                    quotaName = txtQuota + "_" + s.Name;
                    cota.Name = quotaName;
                    cota.Fill.UniformColor.CMYKAssign(0, 0, 0, 0);
                    cota.SetSize(frameWidth-borderDistance);
                    cota.SetPosition(posX+(frameWidth/2),posY+(frameHeight/2));
                    cota.MoveToLayer(lyrQuota);

                    cota.Selected = true;
                    frame.AddToSelection();

                    sGroup = doc.Selection().Group();
                    sGroup.Name = $"{measureName} {txtQuota}_{s.Name}";

                }

                MessageBox.Show($"Done!", "IS Studio", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Execeção não prevista:\n\n {ex}");
            }
        }

        public void CreateSilkScreen(double frameWidth, double frameHeight, double profile, double PrintEngravingHeight)
        {
            if (!DocExists()) { return; }
            if (!SelectExists()) { return; }

            try
            {
                Document doc = corelApp.ActiveDocument;
                Page pag = doc.ActivePage;
                doc.DrawingOriginX = pag.LeftX;
                doc.DrawingOriginY = pag.BottomY;
                doc.Unit = cdrUnit.cdrMillimeter;
                doc.ReferencePoint = cdrReferencePoint.cdrCenter;
                
                ShapeRange sr = doc.SelectionRange;

                string lyrQuoteName = "Tecnica";
                string lyrFrameName = "Telas de Silk";
                string frameName = "Quadro";
                string screenName = "Nylon";
                string frameQuoteProfileName = "Frame Profile Quote";
                string frameQuoteArtName = "Frame Art Quote";
                string frameQuoteNylonName = "Frame Nylon Quote";
                string nameS = string.Empty;
                double stepW = 200;
                double stepH = 200;

                //Cria Camadas
                CreateLayer(lyrQuoteName);
                Layer lyrQuote = pag.Layers.Find(lyrQuoteName);
                CreateLayer(lyrFrameName);
                Layer lyrFrame = pag.Layers.Find(lyrFrameName);
                lyrFrame.Activate();

                foreach (Shape s in sr)
                {
                    s.GetPosition(out double posX, out double posY);
                    
                    nameS = s.Name;
                    if (nameS == null || nameS == "")
                        nameS = "_Sem nome";
                    else
                        nameS += "_";

                    //Calcula deslocamento da posição inferior
                    // Metade da area interna - (Metade da Arte + valor da posicao)
                    double deltaY = ((frameHeight / 2) - profile) - ((s.SizeHeight/2) + PrintEngravingHeight);

                    //Cria Perfil Externo do quadro da tela de silk
                    Shape externProfile = pag.ActiveLayer.CreateRectangle2(posX-(frameWidth/2), (posY-(frameHeight/2)) + deltaY, frameWidth, frameHeight);
                    externProfile.MoveToLayer(lyrFrame);
                    Shape screen = externProfile.Duplicate();

                    //Cria Perfil Interno do quadro da tela de silk
                    Shape internProfile = pag.ActiveLayer.CreateRectangle2(posX - ((frameWidth / 2)-profile), (posY - ((frameHeight / 2) - profile)) + deltaY, frameWidth-(2*profile), frameHeight-(2*profile));
                    internProfile.MoveToLayer(lyrFrame);
                    Shape frameQuoteNylon = internProfile.Duplicate();

                    //Cria Quadro
                    internProfile.Selected = true;
                    externProfile.AddToSelection();
                    Shape frame = doc.Selection().Combine();
                    frame.Outline.Width = 0.25;
                    frame.Outline.Color.CMYKAssign(0, 0, 0, 50);
                    frame.Fill.UniformColor.CMYKAssign(0, 0, 0, 50);
                    frame.Name = $"{frameName}{nameS}";

                    //Cria Tela de nylon
                    Color corTela = new Color();
                    corTela.CMYKAssign(100, 0, 0, 0);
                    //Shape screen = pag.ActiveLayer.CreateRectangle2(posX - ((frameWidth / 2) - profile), posY - ((frameHeight / 2) - profile), frameWidth - (2 * profile), frameHeight - (2 * profile));
                    screen.MoveToLayer(lyrFrame);
                    screen.Outline.SetNoOutline();
                    screen.Fill.UniformColor.CMYKAssign(100, 0, 0, 0);
                    screen.CreateLens(cdrLensType.cdrLensTransparency, 70, corTela);
                    screen.Name = $"{screenName}{nameS}";

                    //Cria Quadro da Cota da arte
                    Shape frameQuoteArt = pag.ActiveLayer.CreateRectangle2(posX - (s.SizeWidth / 2), posY - (s.SizeHeight / 2), s.SizeWidth, s.SizeHeight);
                    frameQuoteArt.MoveToLayer(lyrQuote);
                    frameQuoteArt.Outline.SetNoOutline();
                    frameQuoteArt.Fill.ApplyNoFill();
                    frameQuoteArt.Name = $"{frameQuoteArtName}{nameS}";

                    //Cria Quadro da Cota da Tela de silk
                    Shape frameQuoteProfile = pag.ActiveLayer.CreateRectangle2(posX - (frameWidth / 2), posY - (frameHeight / 2) + deltaY, frameWidth, frameHeight);
                    frameQuoteProfile.MoveToLayer(lyrQuote);
                    frameQuoteProfile.Outline.SetNoOutline();
                    frameQuoteProfile.Fill.ApplyNoFill();
                    frameQuoteProfile.Name = $"{frameQuoteProfileName}{nameS}";

                    //Cria Quadro da Cota do Nylon
                    //Shape frameQuoteNylon = pag.ActiveLayer.CreateRectangle2(posX - ((frameWidth / 2) - profile), posY - ((frameHeight / 2) - profile), frameWidth - (2 * profile), frameHeight - (2 * profile));
                    frameQuoteNylon.MoveToLayer(lyrQuote);
                    frameQuoteNylon.Outline.SetNoOutline();
                    frameQuoteNylon.Fill.ApplyNoFill();
                    frameQuoteNylon.Name = $"{frameQuoteNylonName}{nameS}";

                    //Cria Cotas Tela de silk
                    double distQuota = 35;
                    double posDimensionY = frameQuoteProfile.PositionY + (frameQuoteProfile.SizeHeight / 2) + distQuota; //acima
                    double posDimensionX = frameQuoteProfile.PositionX - (frameQuoteProfile.SizeWidth / 2) - distQuota; //esquerda

                    Shape topDimensionProfileQuote = lyrQuote.CreateLinearDimension(cdrLinearDimensionType.cdrDimensionSlanted,
                                                                    frameQuoteProfile.SnapPoints.Edge(3, 1),
                                                                    frameQuoteProfile.SnapPoints.Edge(2, 1),
                                                                    true, posDimensionX, posDimensionY,
                                                                    Placement: cdrDimensionPlacement.cdrDimensionAboveLine);
                    topDimensionProfileQuote.Name = $"{s.Name}_Largura da Quadro";

                    Shape leftDimensionProfileQuote = lyrQuote.CreateLinearDimension(cdrLinearDimensionType.cdrDimensionSlanted,
                                                                    frameQuoteProfile.SnapPoints.Edge(3, 1),
                                                                    frameQuoteProfile.SnapPoints.Edge(4, 1),
                                                                    true, posDimensionX, posDimensionY,
                                                                    Placement: cdrDimensionPlacement.cdrDimensionAboveLine);
                    leftDimensionProfileQuote.Name = $"{s.Name}_Altura do Quadro";

                    //Cria Cotas desenho
                    double distArtQuote = 35;
                    double posDimensionYArtQuote = frameQuoteArt.PositionY + (frameQuoteArt.SizeHeight / 2) + distArtQuote; //acima
                    double posDimensionXArtQuote = frameQuoteArt.PositionX - (frameQuoteArt.SizeWidth / 2) - distArtQuote; //esquerda

                    Shape topDimensionArtQuote = lyrQuote.CreateLinearDimension(cdrLinearDimensionType.cdrDimensionSlanted,
                                                                    frameQuoteArt.SnapPoints.Edge(3, 1),
                                                                    frameQuoteArt.SnapPoints.Edge(2, 1),
                                                                    true, posDimensionXArtQuote, posDimensionYArtQuote,
                                                                    Placement: cdrDimensionPlacement.cdrDimensionAboveLine);
                    topDimensionArtQuote.Name = $"{s.Name}_Largura da Arte";

                    Shape leftDimensionArtQuote = lyrQuote.CreateLinearDimension(cdrLinearDimensionType.cdrDimensionSlanted,
                                                                    frameQuoteArt.SnapPoints.Edge(3, 1),
                                                                    frameQuoteArt.SnapPoints.Edge(4, 1),
                                                                    true, posDimensionXArtQuote, posDimensionYArtQuote,
                                                                    Placement: cdrDimensionPlacement.cdrDimensionAboveLine);
                    leftDimensionArtQuote.Name = $"{s.Name}_Altura da Arte";

                    //Cria Cotas dos Espaços
                    double distSpaceQuote = 0;
                    double posDimensionXSpaceQuote = frameQuoteArt.PositionX + distSpaceQuote; //centro
                    double posDimensionYSpaceQuote = frameQuoteArt.PositionY + distSpaceQuote; //centro

                    double deltaAnchorY = deltaY / frameQuoteNylon.SizeHeight;

                    Shape topDimensionSpaceQuote = lyrQuote.CreateLinearDimension(cdrLinearDimensionType.cdrDimensionSlanted,
                                                                    frameQuoteNylon.SnapPoints.Edge(3, 0.5),
                                                                    frameQuoteArt.SnapPoints.Edge(3, 0.5),
                                                                    true, posDimensionXSpaceQuote, posDimensionYSpaceQuote,
                                                                    Placement: cdrDimensionPlacement.cdrDimensionAboveLine);

                    topDimensionSpaceQuote.Name = $"{s.Name}_Distancia do Topo";

                    Shape bottomDimensionSpaceQuote = lyrQuote.CreateLinearDimension(cdrLinearDimensionType.cdrDimensionSlanted,
                                                                    frameQuoteNylon.SnapPoints.Edge(1, 0.5),
                                                                    frameQuoteArt.SnapPoints.Edge(1, 0.5),
                                                                    true, posDimensionXSpaceQuote, posDimensionYSpaceQuote,
                                                                    Placement: cdrDimensionPlacement.cdrDimensionAboveLine);
                    bottomDimensionSpaceQuote.Name = $"{s.Name}_Distancia da Base";

                    Shape leftDimensionSpaceQuote = lyrQuote.CreateLinearDimension(cdrLinearDimensionType.cdrDimensionSlanted,
                                                                    frameQuoteNylon.SnapPoints.Edge(4, 0.5+deltaAnchorY),
                                                                    frameQuoteArt.SnapPoints.Edge(4, 0.5),
                                                                    true, posDimensionXSpaceQuote, posDimensionYSpaceQuote,
                                                                    Placement: cdrDimensionPlacement.cdrDimensionAboveLine);
                    leftDimensionSpaceQuote.Name = $"{s.Name}_Distancia da Esquerda";

                    Shape rigthDimensionSpaceQuote = lyrQuote.CreateLinearDimension(cdrLinearDimensionType.cdrDimensionSlanted,
                                                                    frameQuoteNylon.SnapPoints.Edge(2, 0.5-deltaAnchorY),
                                                                    frameQuoteArt.SnapPoints.Edge(2, 0.5),
                                                                    true, posDimensionXSpaceQuote, posDimensionYSpaceQuote,
                                                                    Placement: cdrDimensionPlacement.cdrDimensionAboveLine);
                    rigthDimensionSpaceQuote.Name = $"{s.Name}_Distancia da Direita";

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Execeção não prevista:\n\n {ex}");
            }
        }

        public void CreatOffseArroundTheSelection(int numberOffset=1, double offset=0, double offsetWidth=0.25, string cor="")
        {
            if (!DocExists()) { return; }
            if (!SelectExists()) { return; }

            try
            {
                Document doc = corelApp.ActiveDocument;
                doc.Unit = cdrUnit.cdrMillimeter;
                doc.ReferencePoint = cdrReferencePoint.cdrCenter;
                Page pag = doc.ActivePage;
                ShapeRange sr = doc.SelectionRange;
                
                string lyrOffsetName = "Contornos";
                string frameName = "Offset";
                string txtQuota = string.Empty;

                double frameWidth = 1;
                double frameHeight = 1;
                double posX = 0;
                double posY = 0;

                offset = offset*2;

                CreateLayer(lyrOffsetName);
                Layer lyrOffset = pag.ActiveLayer;

                foreach (Shape s in sr)
                {
                    s.GetPosition(out posX, out posY);
                    s.GetSize(out frameWidth, out frameHeight);

                    for (int i = 1; i <= numberOffset; i++)
                    {
                        Shape frame = pag.ActiveLayer.CreateRectangle2(-frameWidth, 0, frameWidth+offset, frameHeight+offset);

                        string offsetName = $"{frameName} {(i*offset).ToString("F2")}_{s.Name}";
                        frame.Outline.Width = offsetWidth;
                        frame.Outline.Color.CMYKAssign(100, 0, 100, 0);
                        //frame.Fill.UniformColor.CMYKAssign(100, 0, 100, 0);
                        frame.Fill.ApplyNoFill();
                        frame.SetPosition(posX, posY);
                        frame.GetSize(out frameWidth, out frameHeight); // obtem tamanho do proximo frame
                        frame.Name = offsetName;
                        frame.MoveToLayer(lyrOffset);
                    }
                }

                MessageBox.Show($"Done!", "IS Studio", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Execeção não prevista:\n\n {ex}");
            }
        }

        public void OrderObjectsOnLayer(string axis,string direction )
        {
            if (!DocExists()) { return; }
            if (!SelectExists()) { return; }

            try
            {
                Document doc = corelApp.ActiveDocument;
                doc.Unit = cdrUnit.cdrMillimeter;
                doc.ReferencePoint = cdrReferencePoint.cdrCenter;
                Page pag = doc.ActivePage;
                ShapeRange sr = doc.SelectionRange;

                Objetos.Clear();

                foreach (Shape s in sr){ 
                    Objetos objeto = new Objetos(s.StaticID, s.Name, s.PositionX, s.PositionY);
                    Objetos.Add(objeto);
                }

                if (axis == "x")
                {
                    if (direction == "asc")
                    {
                        Objetos = Objetos.OrderBy(o => o.posX).ToList();
                    }
                    else
                    {
                        Objetos = Objetos.OrderByDescending(o => o.posX).ToList();
                    }
                }
                else if (axis == "y")
                {
                    if (direction == "asc")
                    {
                        Objetos = Objetos.OrderBy(o => o.posY).ToList();
                    }
                    else
                    {
                        Objetos = Objetos.OrderByDescending(o => o.posY).ToList();
                    }
                }

                foreach (Objetos objeto in Objetos)
                {
                    Shape s = pag.FindShape(null, cdrShapeType.cdrNoShape, objeto.Id);
                    if (s!=null)
                    {
                        //s.CreateSelection();
                       //MessageBox.Show($"Move {s.Name} para o inicio");
                        s.OrderToFront();
                    }
                    else
                    {
                        MessageBox.Show("Nada encontrado");
                    }
                }

                //string listaComoString = "";
                //foreach (Objetos objeto in Objetos)
                //{
                //    listaComoString += $"Id: {objeto.Id}, Nome: {objeto.Name}, posX: {objeto.posX}, posY: {objeto.posY}\n";
                //}
                //MessageBox.Show(listaComoString);

                MessageBox.Show($"Done!", "IS Studio", MessageBoxButton.OK, MessageBoxImage.Information);
                
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Execeção não prevista:\n\n {ex}");
            }
        }

        public void CreateBoundaryMarks(double markSize)
        {
            if (!DocExists()) { return; }
            if (!SelectExists()) { return; }

            try
            {
                Document doc = corelApp.ActiveDocument;
                doc.Unit = cdrUnit.cdrMillimeter;
                doc.ReferencePoint = cdrReferencePoint.cdrCenter;
                Page pag = doc.ActivePage;
                ShapeRange sr = doc.SelectionRange;

                string msg = string.Empty;
                double sizeX;
                double sizeY;
                double posX;
                double posY;
                foreach (Shape s in sr)
                {
                    string name = s.Name;
                    s.GetSize(out sizeX, out sizeY);
                    s.GetPosition(out posX, out posY);

                    double Left = posX-(sizeX / 2);
                    double Right = posX + (sizeX / 2);
                    double Top = posY + (sizeY / 2);
                    double Bottom = posY - (sizeY / 2);

                    //Mark Center
                    //Shape c = pag.ActiveLayer.CreateEllipse2(posX, posY, 2);
                    //c.Fill.UniformColor.CMYKAssign(0, 100, 100, 0);

                    //Mark Top-Left
                    Shape cTopLeft_Right = pag.ActiveLayer.CreateLineSegment(Left, Top, Left + markSize, Top);
                    Shape cTopLeft_Down = pag.ActiveLayer.CreateLineSegment(Left, Top, Left, Top - markSize);
                    cTopLeft_Right.Outline.Color.CMYKAssign(0, 100, 100, 0);
                    cTopLeft_Down.Outline.Color.CMYKAssign(0, 100, 100, 0);
                    cTopLeft_Right.AddToSelection();
                    Shape MarkTopLeft = doc.SelectionRange.Combine();
                    MarkTopLeft.Name = "Mark Top Left";

                    //Mark Bottom-Left
                    Shape cBottomLeft_Right = pag.ActiveLayer.CreateLineSegment(Left, Bottom, Left + markSize, Bottom);
                    Shape cBottomLeft_Top = pag.ActiveLayer.CreateLineSegment(Left, Bottom, Left, Bottom + markSize);
                    cBottomLeft_Right.Outline.Color.CMYKAssign(0, 100, 100, 0);
                    cBottomLeft_Top.Outline.Color.CMYKAssign(0, 100, 100, 0);
                    cBottomLeft_Right.AddToSelection();
                    Shape MarkBottomLeft = doc.SelectionRange.Combine();
                    MarkBottomLeft.Name = "Mark Bottom Left";

                    //Mark Top-Right
                    Shape cTopRigth_Left = pag.ActiveLayer.CreateLineSegment(Right, Top, Right - markSize, Top);
                    Shape cTopRigth_Down = pag.ActiveLayer.CreateLineSegment(Right, Top, Right, Top - markSize);
                    cTopRigth_Left.Outline.Color.CMYKAssign(0, 100, 100, 0);
                    cTopRigth_Down.Outline.Color.CMYKAssign(0, 100, 100, 0);
                    cTopRigth_Left.AddToSelection();
                    Shape MarkTopRight = doc.SelectionRange.Combine();
                    MarkTopRight.Name = "Mark Top Right";


                    //Mark Bottom-Right
                    Shape cBottomRigth_Left = pag.ActiveLayer.CreateLineSegment(Right, Bottom, Right - markSize, Bottom);
                    Shape cBottomRigth_Top = pag.ActiveLayer.CreateLineSegment(Right, Bottom, Right, Bottom + markSize);
                    cBottomRigth_Left.Outline.Color.CMYKAssign(0, 100, 100, 0);
                    cBottomRigth_Top.Outline.Color.CMYKAssign(0, 100, 100, 0);
                    cBottomRigth_Left.AddToSelection();
                    Shape MarkBottomRight = doc.SelectionRange.Combine();
                    MarkBottomRight.Name = "Mark Bottom Right";

                    //Make string matrix
                    //msg += $"{name}\n x:{sizeX}, y:{sizeY}\nposX:{posX}, posY:{posY}\n\n";
                }
                MessageBox.Show($"Done!\n {msg}", "IS Studio", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {

                MessageBox.Show($"Execeção não prevista:\n\n {ex}");
            }
        }
    }
}
