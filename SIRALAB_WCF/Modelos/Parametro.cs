using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace SIRALAB_WCF.Modelos
{
    [DataContract(IsReference = true)]
    public class Parametro
    {
        [DataMember]
        public Parametros Param { get; set; }
        [DataMember]
        public decimal Valor { get; set; }
        [DataMember]
        public bool ValorValidacion { get; set; }
        
        [DataMember]
        public string RFCLaboratorioMuentra { get; set; }
        [DataMember]
        public string Metodo { get; set; }
        [DataMember]
        public Accion Accion { get; set; }
        [DataMember]
        public decimal LC_MC { get; set; }

        }
    [DataContract]
    public enum Accion
    {
        [EnumMember]
        SinAccion = 0,
        [EnumMember]
        Exento = 1,
        [EnumMember]
        NoAnalizado = 2,
    }

        [DataContract]
    public enum Parametros
    {
        [EnumMember]
        _1_1_1_Tricloroetano = 129,
        [EnumMember]
        _1_1_2_2_Tetracloroetano = 133,
        [EnumMember]
        _1_1_2_Tricloroetano = 131,
       
         [EnumMember]_1_1_Dicloroetileno = 134,
         [EnumMember]_1_2_Diclorobenceno = 135,
         [EnumMember]_1_2_Dicloroetano = 136,
         [EnumMember]_1_3_Diclorobenceno = 143,
         [EnumMember]_1_4_Diclorobenceno = 145,
         [EnumMember]_2_4_5_Triclorofenol = 150,
         [EnumMember]_2_4_6__Triclorofenol = 153,
         [EnumMember]_2_4_Dinitrofenol = 155,
         [EnumMember]_2_4_Dinitrotolueno = 148,
         [EnumMember]_Clordano = 217,
         [EnumMember]_Xilenos = 317,
         [EnumMember]_1_1_2_2_Tetracloroetano_ = 4,
         [EnumMember]_1_1_Dicloroetileno_ = 1,
         [EnumMember]_1_2__Diclorobenceno = 345,
         [EnumMember]_1_2_Dicloropropano_ = 7,
         [EnumMember]_1_2_Dicloropropano = 360,
         [EnumMember]_1_2_Dicloropropileno_ = 8,
         [EnumMember]_1_2_Difenilhidracina_ = 9,
         [EnumMember]_2_3_7_8__tetaclorodibenzo_p_dioxina = 378,
         [EnumMember]_2_4_6_Triclorofenol_ = 15,
         [EnumMember]_2_4_Diclorofenol_ = 11,
         [EnumMember]_2_4_Diclorofenol = 395,
         [EnumMember]_2_4_Dimetilfenol_ = 12,
         [EnumMember]_2_4_Dinitrofenol_ = 13,
         [EnumMember]_2_4_Dinitrotolueno_ = 14,
         [EnumMember]_2_6_Dinitrotolueno_ = 16,
         [EnumMember]_2_Cloroetil_Vinil_Éter_ = 17,
         [EnumMember]_2_Clorofenol = 18,
         [EnumMember]_4_Bromofenil_Fenil_Eter = 426,
         [EnumMember]Acenafteno_ = 22,
         [EnumMember]Acido_2_4_Diclorofenoxiacetico = 449,
         [EnumMember]Acrilonitrilo = 24,
         [EnumMember]Acroleína_ = 25,
         [EnumMember]Alcalinidad_ = 26,
         [EnumMember]Aldrín_ = 27,
         [EnumMember]Aluminio = 472,
         [EnumMember]Antimonio_ = 29,
         [EnumMember]Arsenico = 30,
         [EnumMember]Arsenico1 = 966,
         [EnumMember]B2_Etilhexil_ftalato = 494,
         [EnumMember]Bario_ = 32,
         [EnumMember]Benceno_ = 33,
         [EnumMember]Benceno = 499,
         [EnumMember]Bencidina_ = 34,
         [EnumMember]Berilio_ = 35,
         [EnumMember]Bifenilos_policlorados_ = 38,
         [EnumMember]Bis_2_Cloroetil_Éter_ = 39,
         [EnumMember]Bis_2_Cloroisopropil_Éter_ = 40,
         [EnumMember]Bis_2_Etilhexil_Ftalato_ = 41,
         [EnumMember]Boro_ = 42,
         [EnumMember]Bromoformo_ = 43,
         [EnumMember]Bromuro_de_Metilo_ = 44,
         [EnumMember]Cadmio_ = 45,
         [EnumMember]Cianuro = 555,
         [EnumMember]Cianuros = 556,
         [EnumMember]Clordano = 564,
         [EnumMember]Clorobenceno_ = 50,
         [EnumMember]Cloroformo_ = 51,
         [EnumMember]Cloruro_de_Metileno_ = 53,
         [EnumMember]Cloruro_de_Metilo_ = 54,
         [EnumMember]Cloruro_de_Vinilo_ = 55,
         [EnumMember]Cloruros = 591,
         [EnumMember]Cobre_ = 57,
         [EnumMember]Coliformes_Fecales = 225,
         [EnumMember]Coliformes_Totales = 596,
         [EnumMember]Color_Platino_Cobalto = 599,
         [EnumMember]Conductividad_eléctrica = 1018,
         [EnumMember]Conductividad = 601,
         [EnumMember]Cromo_Hexavalente = 607,
         [EnumMember]Cromo_Total_ = 60,
         [EnumMember]Cromo = 606,
         [EnumMember]DBO5 = 618,
         [EnumMember]DDD_Diclorofenildicloroetano_ = 61,
         [EnumMember]Diclorobenceno = 640,
         [EnumMember]Dieldrín_ = 66,
         [EnumMember]Dietil_ftalato = 648,
         [EnumMember]Dietilftalato_ = 67,
         [EnumMember]Dimetilftalato_ = 68,
         [EnumMember]Dinitro_o_Cresol_ = 69,
         [EnumMember]DQO = 667,
         [EnumMember]Dureza_Total = 672,
         [EnumMember]Endosulfan_Alfa_y_Beta_ = 70,
         [EnumMember]Endrín_ = 71,
         [EnumMember]Etil_benceno = 702,
         [EnumMember]Etilbenceno_ = 72,
         [EnumMember]Fenol_ = 73,
         [EnumMember]Fenoles_Totales = 713,
         [EnumMember]Fierro_ = 74,
         [EnumMember]Fluoranteno_ = 75,
         [EnumMember]Fluoruros = 723,
         [EnumMember]Fósforo_Total_ = 77,
         [EnumMember]Grasas_y_Aceites_ = 79,
         [EnumMember]HAP_S = 741,
         [EnumMember]Heptacloro_ = 81,
         [EnumMember]Hexaclorobenceno_ = 82,
         [EnumMember]Hexaclorobutadieno_ = 83,
         [EnumMember]Hexaclorociclopentadieno_ = 84,
         [EnumMember]Hexacloroetano_ = 85,
         [EnumMember]Huevos_de_Helminto = 767,
         [EnumMember]Manganeso_ = 88,
         [EnumMember]Materia_Flotante = 89,
         [EnumMember]Mercurio_ = 90,
         [EnumMember]Metoxicloro_ = 91,
         [EnumMember]N_Nitrosodi_N_Propilamina_ = 100,
         [EnumMember]N_Nitrosodifenilamina_ = 98,
         [EnumMember]N_Nitrosodimetilamina_ = 99,
         [EnumMember]Naftaleno_ = 92,
         [EnumMember]Níquel_ = 93,
         [EnumMember]Nitratos_NO3_como_N = 94,
         [EnumMember]Nitratos = 828,
         [EnumMember]Nitritos = 829,
         [EnumMember]Nitrobenceno_ = 96,
         [EnumMember]Nitrógeno_Amoniacal_como_N_ = 97,
         [EnumMember]Nitrógeno_Amoniacal = 831,
         [EnumMember]Nitrógeno_Kjeldahl = 836,
         [EnumMember]Nitrógeno_Total = 835,
         [EnumMember]Olor_ = 101,
         [EnumMember]Oxígeno_Disuelto_ = 102,
         [EnumMember]Paration_ = 103,
         [EnumMember]Pentaclorofenol_ = 104,
         [EnumMember]pH = 862,
         [EnumMember]Plata_ = 105,
         [EnumMember]Plomo__ = 106,
         [EnumMember]SAAM = 881,
         [EnumMember]SAAM1 = 885,
         [EnumMember]Selenio = 888,
         [EnumMember]Sólidos_Disueltos_Totales_ = 111,
         [EnumMember]Sólidos_Sedimentables = 298,
         [EnumMember]Sólidos_Suspendidos_Totales_ = 112,
         [EnumMember]Sólidos_Totales_ = 113,
         [EnumMember]Sulfatos = 906,
         [EnumMember]Sulfuros = 909,
         [EnumMember]Talio_ = 117,
         [EnumMember]Temperatura_C = 118,
         [EnumMember]Tetracloroetileno_ = 119,
         [EnumMember]Tetracloruro_de_Carbono_ = 120,
         [EnumMember]Tolueno_ = 121,
         [EnumMember]Toxafeno_ = 122,
         [EnumMember]Toxicidad = 931,
         [EnumMember]Turbiedad = 955,
         [EnumMember]Xilenos = 962,
         [EnumMember]Zinc = 126,
    }
}