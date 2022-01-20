import {Aerodrom} from "./Aerodrom.js";
import {Kompanija} from "./Kompanija.js";
import {Destinacija} from "./Destinacija.js";
import { Putnik } from "./Putnik.js";
import { Let } from "./Let.js";


// ----------------------------------------------- AERODROM - PREUZIMANJE -------------------------------------------------
var listaTipovaSedista=[];
fetch("https://localhost:5001/Putnik/PreuzmiTipoveSedista")
    .then(p=>{
        p.json().then(tipovi=>{
            tipovi.forEach(tip => {
                if(!listaTipovaSedista.includes(tip))
                 listaTipovaSedista.push(tip);
            })
           
        }) 
    })

    
var listaLetova=[];
fetch("https://localhost:5001/Let/PreuzmiLetoveZaAerodrom")
.then(p=>{
    p.json().then(letovi=>{
        letovi.forEach(le => {
            var l= new Let(le.tipSedista, le.cena, le.aerodrom.id); 
            listaLetova.push(l);
        })
    }) 
})

var listaPutnika=[];
fetch("https://localhost:5001/Putnik/PreuzmiPutnikeZaAerodrom")
.then(p=>{
    p.json().then(putnici=>{
        putnici.forEach(p => {
            console.log("IME: " + p.ime)
            var pu= new Putnik(p.ime,p.prezime,p.email,p.jmbg,p.brLicneKarte,p.brTelefona,p.aerodrom.id); 
            listaPutnika.push(pu);
        })
    }) 
})


var listaDestinacija =[];
fetch("https://localhost:5001/Destinacija/PreuzmiDestinacijeZaAerodrom")
.then(p=>{
    p.json().then(desti=>{
        desti.forEach(de => {
            var p= new Destinacija(de.id, de.kontinent,  de.drzava,  de.grad,  de.datumiVreme, de.aerodrom.id); 
                  listaDestinacija.push(p);
        })
    }) 
})

var listaKompanija =[];
fetch("https://localhost:5001/Kompanija/PreuzmiKompanijeZaAerodrom")
.then(r=>{
    r.json().then(kompa=>{
        kompa.forEach(komp => {
            var k = new Kompanija(komp.id, komp.imeKompanije,komp.brSedista, komp.aerodrom.id);
            listaKompanija.push(k);    
        })
    })
})

var listaAerodroma =[];
fetch("https://localhost:5001/Aerodrom/PreuzmiAerodrome")
.then(p=>{
    p.json().then(aero=>{
        aero.forEach(ae=> {
            var a =new Aerodrom(ae.id, ae.imeAerodroma, ae.imeGrada, listaTipovaSedista);
            listaAerodroma.push(a);

            listaLetova.forEach(l=>{
                if(l.aerodromId==ae.id)
                a.dodajLet(l);
            })

            listaPutnika.forEach(l=>{
                if(l.aerodromId==ae.id)
                a.dodajPutnikaa(l);
            })

            listaKompanija.forEach(l=>{
                if(l.aerodromId==ae.id)
                a.dodajKompaniju(l);
            })

            listaDestinacija.forEach(l=>{
                if(l.aerodromId==ae.id)
                   a.dodajDestinaciju(l);
            })
            a.crtaj(document.body);
        })

    }) 
})


console.log(listaAerodroma)
