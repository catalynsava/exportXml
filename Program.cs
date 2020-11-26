using System;
using System.Data;
using System.Data.OleDb;
using exportXml.Exporturi;

namespace exportXml
{
    class Program
    {
        static void Main(string[] args)
        {
            string parametru="";
            foreach (string  item in args)
            {
                parametru+=item + " ";
            }
            Console.WriteLine(parametru);

            BazaDeDate.nume=args[0];
            BazaDeDate.conectare(args[0]);

            if(args[1]=="-validare"){
                switch (args[2])
                {
                    case "cnp":
                        if(args[3]=="adrrolcnpuri"){
                            Validari.CnpValidare.adrrolcnpuri();
                        }
                        if(args[3]=="cap1cnpuri"){
                            Validari.CnpValidare.cap1cnpuri();
                        }
                        if(args[3]=="cap2bcnpuri"){
                            Validari.CnpValidare.cap2bcnpuri();
                        }
                        if(args[3]=="cap13cnpuri"){
                            Validari.CnpValidare.cap13cnpuri();
                        }
                        if(args[3]=="succesoricnpuri"){
                            Validari.CnpValidare.succesoricnpuri();
                        }
                        if(args[3]=="cumparatoricnpuri"){
                            Validari.CnpValidare.cumparatoricnpuri();
                        }
                        if(args[3]=="cap15acnpuri"){
                            Validari.CnpValidare.cap15acnpuri();
                        }
                        if(args[3]=="cap15bcnpuri"){
                            Validari.CnpValidare.cap15bcnpuri();
                        }
                        if(args[3]=="uncnpcateroluri"){
                            Validari.CnpValidare.uncnpcateroluri();
                        }
                        if(args[3]=="uncnpcatimembri"){
                            Validari.CnpValidare.cnpuricatimembri();
                        }
                        break;
                    case "cif":
                        break;
                    default:
                        break;
                }
            }

            if(args[1]=="-export"){
                if(args[2]=="CAP0_12"){
                    Export.comun("CAP0_12");
                    Export.cap0_12();
                }
                if(args[2]=="CAP0_34"){
                    Export.comun("CAP0_34");
                    Export.cap0_34();
                }
                 if(args[2]=="CAP1"){
                    Export.comun("CAP1");
                    Export.cap1();
                }
                if(args[2]=="CAP2a"){
                    Export.comun("CAP2a");
                    Export.cap2a();
                }
                if(args[2]=="CAP2b"){
                    Export.comun("CAP2b");
                    Export.cap2b();
                }
                if(args[2]=="CAP3"){
                    Export.comun("CAP3");
                    Export.cap3();
                }
                if(args[2]=="CAP4a"){
                    Export.comun("CAP4a");
                    Export.cap4a();
                }
                if(args[2]=="CAP4a1"){
                    Export.comun("CAP4a1");
                    Export.cap4a1();
                }
                if(args[2]=="CAP4b1"){
                    Export.comun("CAP4b1");
                    Export.cap4b1();
                }
                if(args[2]=="CAP4b2"){
                    Export.comun("CAP4b2");
                    Export.cap4b2();
                }
                if(args[2]=="CAP4c"){
                    Export.comun("CAP4c");
                    Export.cap4c();
                }
                if(args[2]=="CAP5a"){
                    Export.comun("CAP5a");
                    Export.cap5a();
                }
                if(args[2]=="CAP5b"){
                    Export.comun("CAP5b");
                    Export.cap5b();
                }
                if(args[2]=="CAP5c"){
                    Export.comun("CAP5c");
                    Export.cap5c();
                }
                if(args[2]=="CAP5d"){
                    Export.comun("CAP5d");
                    Export.cap5d();
                }
                if(args[2]=="CAP6"){
                    Export.comun("CAP6");
                    Export.cap6();
                }
                if(args[2]=="CAP7"){
                    Export.comun("CAP7");
                    if(args[3]=="1"){
                        Export.cap7(1);
                    }else if(args[3]=="2"){
                        Export.cap7(2);
                    }else{
                        Export.cap7(1);
                    }
                }
                if(args[2]=="CAP8"){
                    Export.comun("CAP8");
                    if(args[3]=="1"){
                        Export.cap8(1);
                    }else if(args[3]=="2"){
                        Export.cap8(2);
                    }else{
                        Export.cap8(1);
                    }
                }
                if(args[2]=="CAP9"){
                    Export.comun("CAP9");
                    Export.cap9();
                }
                if(args[2]=="CAP11"){
                    Export.comun("CAP11");
                    Export.cap11();
                }
                if(args[2]=="CAP12"){
                    Export.comun("CAP12");
                    Export.cap12();
                }
                if(args[2]=="CAP15a"){
                    Export.comun("CAP15a");
                    Export.cap15a();
                }
                if(args[2]=="CAP15b"){
                    Export.comun("CAP15b");
                    Export.cap15b();
                }
                 if(args[2]=="MENTIUNI"){
                    Export.comun("MENTIUNI");
                    Export.mentiuni();
                }
            }

             if(args[1]=="-exportfisiere"){
                  if(args[2]=="CAP1"){
                    Util.comun("CAP1");
                    Util.cap1();
                }
             }
            BazaDeDate.deconectare();
        }
    }
}
