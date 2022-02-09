using System;
using System.Collections.Generic;

// Classes serializadas para representar os dados do JSON inicial
[Serializable]
public class Carta {

    public string imagemCarta;

}

[Serializable]
public class Config
{
    
    public List<Carta> cartas;

    public float tempoTotal;
    
}