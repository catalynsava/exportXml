namespace exportXml.Validari
{
    public class CifValidare
    {
        public static RaspunsValidare verificaCIF(string strCIF)
        {
            RaspunsValidare raspuns=new RaspunsValidare();
            string strInitialCIF;
            strInitialCIF = strCIF;
            int cifradecontrol, suma, intCIF;

            if (strCIF.Length > 2 && strCIF.Substring(0, 2) == "RO")
            {
                strCIF = strCIF.Substring(2, strCIF.Length - 2);
                strCIF=strCIF.ToString().Trim();
                
            } // strCIF fără "RO"
            if (strCIF.Length > 10)
            {
                raspuns.corect=false;
                raspuns.detalii="CIF:\"" + strInitialCIF + "\" invalid, are mai mult de 10 cifre.";;
                Ajutatoare.scrielinie("cifuriInvalidate.log",raspuns.detalii);
                return raspuns;
            }// mai mare ca 10
            if (strCIF.Length < 4)
            {
                raspuns.corect=false;
                raspuns.detalii= "CIF:\"" + strInitialCIF + "\" invalid, are mai puțin de 4 cifre.";
                Ajutatoare.scrielinie("cifuriInvalidate.log",raspuns.detalii);
                return raspuns;
            }// mai mare ca 10
            if (!int.TryParse(strCIF, out intCIF))
            {
                raspuns.corect=false;
                raspuns.detalii= "CIF-:\"" + strInitialCIF + "\" invalid, nu conține numai caractere numerice.";
                Ajutatoare.scrielinie("cifuriInvalidate.log",raspuns.detalii);
                return raspuns;
            }
            cifradecontrol = int.Parse(strCIF.Substring(strCIF.Length - 1, 1));
            strCIF = strCIF.Substring(0, strCIF.Length - 1);
            if (strCIF.Length < 9)
            {
                strCIF = new string('0', 9 - strCIF.Length) + strCIF;
            }
            suma = 0;
            for (int x = 0; x <= 8; x++)
            {
                switch (x)
                {
                    case 0:
                        suma += int.Parse(strCIF.Substring(0, 1)) * 7;
                        break;
                    case 1:
                        suma += int.Parse(strCIF.Substring(1, 1)) * 5;
                        break;
                    case 2:
                        suma += int.Parse(strCIF.Substring(2, 1)) * 3;
                        break;
                    case 3:
                        suma += int.Parse(strCIF.Substring(3, 1)) * 2;
                        break;
                    case 4:
                        suma += int.Parse(strCIF.Substring(4, 1));
                        break;
                    case 5:
                        suma += int.Parse(strCIF.Substring(5, 1)) * 7;
                        break;
                    case 6:
                        suma += int.Parse(strCIF.Substring(6, 1)) * 5;
                        break;
                    case 7:
                        suma += int.Parse(strCIF.Substring(7, 1)) * 3;
                        break;
                    case 8:
                        suma += int.Parse(strCIF.Substring(8, 1)) * 2;
                        break;
                }
            }
            suma *= 10;
            suma %= 11;
            if (suma == 10)
            {
                suma = 0;
            }
            if ((cifradecontrol == suma))
            {
                raspuns.corect=true;
                raspuns.detalii= "CIF:\"" + strInitialCIF + "\" este valid.";
                return raspuns;
            }
            else
            {
                raspuns.corect=false;
                raspuns.detalii="CIF:\"" + strInitialCIF + "\" cheie de control invalidă.";
                Ajutatoare.scrielinie("cifuriInvalidate.log",raspuns.detalii);
                return raspuns;
            }
        }
    }
}