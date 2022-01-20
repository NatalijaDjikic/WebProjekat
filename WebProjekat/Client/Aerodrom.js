import {Kompanija} from "./Kompanija.js";
import {Destinacija} from "./Destinacija.js";
import {PutnikZaPrikaz} from "./PutnikZaPrikaz.js";
import{Let} from "./Let.js";
import{Putnik} from "./Putnik.js";

export class Aerodrom
{
    constructor(id,imeAerodroma,imeGrada,listaTipovaSedista)
    {
        this.id=id;
        this.imeAerodroma=imeAerodroma;
        this.imeGrada=imeGrada;

        this.listaLetova=[];
        this.listaDestinacija=[];
        this.listaKompanija=[];
        this.listaPutnika=[];
        this.kont=null;

        // za select
        this.listaTipovaSedista=listaTipovaSedista;
      
        this.listaRazlicitihDestinacija=[];
        this.listaRazlicitihDatuma=[];
    }

    dodajLet(l)
    {
        this.listaLetova.push(l);
    }
    dodajDestinaciju(d)
    {
        this.listaDestinacija.push(d);
    }
    dodajKompaniju(k)
    {
        this.listaKompanija.push(k);
    }
    dodajPutnikaa(p)
    {
        this.listaPutnika.push(p);
    }
    // --------------------------------- FJE ZA CRTANJE ------------------------------------
    crtaj(host) 
    {
        var l =  document.createElement("h3");
        l.innerHTML=this.imeAerodroma;
        l.className="Naslov";
        host.appendChild(l);

        this.kont=document.createElement("div");
        this.kont.className="GlavniKontejner";
        this.kont.classList.add("kont");
        host.appendChild(this.kont);

        let kontForma = document.createElement("div");
        kontForma.className="Forma";
        this.kont.appendChild(kontForma);
        
    
        this.crtajFormu(kontForma);
        this.crtajPrikaz(kontForma);

        let kontDodaj = document.createElement("div");
        kontDodaj.className="Dodavanje";
        this.kont.appendChild(kontDodaj);

        
        let kontIzbrisi = document.createElement("div");
        kontIzbrisi.className="Brisanje";
        this.kont.appendChild(kontIzbrisi);

        
        let kontIzmena = document.createElement("div");
        kontIzmena.className="Izmena";
        this.kont.appendChild(kontIzmena);

        this.CrtajZaDodavanjePutnika(kontDodaj);
        
        this.CrtajZaObrisiPutnika(kontIzbrisi);
        this.CrtajZaIzmeniTipSedista(kontIzmena);
        
        this.crtajGraf(this.kont);
    }

    crtajPrikaz(host){

        let kontPrikaz = document.createElement("div");
        kontPrikaz.className="Prikaz";
        host.appendChild(kontPrikaz);

        var tabela = document.createElement("table");
        tabela.className="tabela";
        kontPrikaz.appendChild(tabela);

        var tabelahead= document.createElement("thead");
        tabela.appendChild(tabelahead);

        var tr = document.createElement("tr");
        tabelahead.appendChild(tr);

        var tabelaBody = document.createElement("tbody");
        tabelaBody.className="TabelaPodaci" + this.id;
        tabela.appendChild(tabelaBody);

        let th;
        var zag=["Ime", "Prezime","JMBG", "Broj licne karte", "Ime kompanije", "Tip sedista", "Cena"];
        zag.forEach(el=>{
            th = document.createElement("th");
            th.innerHTML=el;
            tr.appendChild(th);
        })
    }

    crtajRed(host){
        let red = document.createElement("div");
        red.className="red";
        host.appendChild(red);
        return red;
    }
     
    // ----------------------------------- DEO ZA PRIKAZ ------------------------------------
    crtajFormu(host){

        //odaberi grad labela
        let red = this.crtajRed(host);
        let l = document.createElement("label");
        l.innerHTML="Odaberi grad: ";
        l.className="Odaberi";
        red.appendChild(l);

        //select za grad
        let se = document.createElement("select");
        se.className="SelectZaGrad";
        red.appendChild(se);
        
       
        this.listaDestinacija.forEach(d=>{
           if(!this.listaRazlicitihDestinacija.includes(d.grad)) 
                this.listaRazlicitihDestinacija.push(d.grad)
        })

        console.log("RAZL: "+this.listaRazlicitihDestinacija)

        let op;
        this.listaRazlicitihDestinacija.forEach(d=>{
            op = document.createElement("option");
            op.innerHTML=d;
            op.value=d;
            se.appendChild(op);
        })

         //odaberi datum labela
        red = this.crtajRed(host);
        l = document.createElement("label");
        l.innerHTML="Odaberi datum:";
        l.className="Odaberi";
        red.appendChild(l);

        //select za datum
        se = document.createElement("select");
        se.className="SelectZaDatum";
        red.appendChild(se);

       
        this.listaDestinacija.forEach(d=>{
           if(!this.listaRazlicitihDatuma.includes(d.datumiVreme)) 
               this.listaRazlicitihDatuma.push(d.datumiVreme)
        })
        
        this.listaRazlicitihDatuma.forEach(d=>{
            op = document.createElement("option");
            op.innerHTML=d;
            op.value=d;
            se.appendChild(op);
        })
        
         //dugme nadji putnike
        red = this.crtajRed(host);
        let btnNadji = document.createElement("button");
        btnNadji.onclick=(ev)=>this.nadjiPutnike();
        btnNadji.innerHTML="Nadji putnike";
        btnNadji.className="Dugme";
        red.appendChild(btnNadji);
        
    }
    
    crtajGraf(host)
    {
        let kontGraf = document.createElement("div");
        kontGraf.className="Graf";
        host.appendChild(kontGraf);

        let red = this.crtajRed(host);
        let l = document.createElement("label");
        l.innerHTML=" ";
        l.className="Statistika";
        red.appendChild(l);

        var FC=0;
        var EC=0;
        var BC=0;
        this.listaLetova.forEach(lett=> { 
            if(lett.tipSedista=="FC")
                FC++;
            else if(lett.tipSedista=="EC")
                EC++;
            else if(lett.tipSedista=="BC")
                BC++;
        })

        var max;
        if(FC>EC)
            max=FC;
        else max=EC;
        if(BC>max)
            max=BC;

        console.log("FC: " + FC)
        let stubici = document.createElement("div");
        stubici.className="Stubici";
        kontGraf.appendChild(stubici);

        // FC
        let stubFC = document.createElement("div");
        stubFC.className="stubFC";
        stubici.appendChild(stubFC);
        stubFC.style.height=(FC*100)/max +"%";
        if(FC!=0)
         stubFC.innerHTML="FC";

        // BC
        let stubBC = document.createElement("div");
        stubBC.className="stubBC";
        stubici.appendChild(stubBC);
        stubBC.style.height=(BC*100)/max +"%";
        if(BC!=0)
         stubBC.innerHTML="BC";

        // EC
        let stubEC = document.createElement("div");
        stubEC.className="stubEC";
        stubici.appendChild(stubEC);
        stubEC.style.height=(EC*100)/max +"%";
        if(EC!=0)
            stubEC.innerHTML="EC";

    }
    nadjiPutnike(){

        let optionEl = this.kont.querySelector(".SelectZaGrad");
        var gradID=optionEl.options[optionEl.selectedIndex].value;
        console.log(gradID);

        optionEl = this.kont.querySelector(".SelectZaDatum");
        var datumiVremeID=optionEl.options[optionEl.selectedIndex].value;
        console.log(datumiVremeID);
       
        this.ucitajPutnike(datumiVremeID,gradID);
    }

    ucitajPutnike(datumiVremeID,gradID){

    fetch("https://localhost:5001/Putnik/PrikaziPutnikeKojiPutuju/"+datumiVremeID+"/"+gradID +"/"+this.id,
    {
        method:"GET"
    }).then(p=>{
        if(p.ok){
            var teloTabele = this.obrisiPrethodniSadrzaj();
            p.json().then(data=>{
                data.forEach(p=>{
                    console.log(p.ime,p.prezime,p.jmbg,p.brLicneKarte,p.imeKompanije, p.tipSedista, p.cena)
                    let put = new PutnikZaPrikaz(p.ime,p.prezime,p.jmbg,p.brLicneKarte,p.imeKompanije, p.tipSedista, p.cena);
                    put.crtaj(teloTabele);
                })
                
            })
        }
    })
}

    obrisiPrethodniSadrzaj(){
        var teloTabele = document.querySelector(".TabelaPodaci"+ this.id);
        var roditelj = teloTabele.parentNode;
        roditelj.removeChild(teloTabele);

        teloTabele = document.createElement("tbody");
        teloTabele.className="TabelaPodaci"+ this.id;
        roditelj.appendChild(teloTabele);
        return teloTabele;
    }

    obrisiPrethodniSadrzajZaGraf(){
        var graf = document.querySelector(".Graf");
        var roditelj = graf.parentNode;
        roditelj.removeChild(graf);
    }

// --------------------------------- DODAVANJE PUTNIKA I LETA -----------------------------
CrtajZaDodavanjePutnika(host)
{
    // Ime
    var red = this.crtajRed(host);
    var l =  document.createElement("label");
    l.innerHTML="Ime:"
    red.appendChild(l);
    var Ime = document.createElement("input");
    Ime.type="string";
    Ime.className="KlasaIme";
    red.appendChild(Ime);

    // Prezime
    red = this.crtajRed(host);
    l =  document.createElement("label");
    l.innerHTML="Prezime:"
    red.appendChild(l);
    var Prezime = document.createElement("input");
    Prezime.type="string";
    Prezime.className="KlasaPrezime";
    red.appendChild(Prezime);

    // JMBG
    red = this.crtajRed(host);
    l =  document.createElement("label");
    l.innerHTML="JMBG:"
    red.appendChild(l);
    var Jmbg = document.createElement("input");
    Jmbg.type="string";
    Jmbg.className="KlasaJMBG";
    red.appendChild(Jmbg);

    // Broj licne karte
    red = this.crtajRed(host);
    l =  document.createElement("label");
    l.innerHTML="Broj licne karte:"
    red.appendChild(l);
    var BrojLicneKarte = document.createElement("input");
    BrojLicneKarte.type="string";
    BrojLicneKarte.className="KlasaBrojLicneKarte";
    red.appendChild(BrojLicneKarte);

    // Broj telefona
    red = this.crtajRed(host);
    l =  document.createElement("label");
    l.innerHTML="Broj telefona:"
    red.appendChild(l);
    var BrojTelefona = document.createElement("input");
    BrojTelefona.type="string";
    BrojTelefona.className="KlasaBrojTelefona";
    red.appendChild(BrojTelefona);

    // Email
    red = this.crtajRed(host);
    l =  document.createElement("label");
    l.innerHTML="Email:"
    red.appendChild(l);
    var Email = document.createElement("input");
    Email.type="string";
    Email.className="KlasaEmail";
    red.appendChild(Email);

    // Odaberi grad
    red = this.crtajRed(host);
    l = document.createElement("label");
    l.innerHTML="Odaberi grad:";
    l.className="Odaberi";
    red.appendChild(l);

    let se = document.createElement("select");
    se.className="SelectZaGrad1";
    red.appendChild(se);

    let op;
    this.listaRazlicitihDestinacija.forEach(d=>{
    op = document.createElement("option");
    op.innerHTML=d;
    op.value=d;
    se.appendChild(op);})    

    // Odaberi datum
    red = this.crtajRed(host);
    l = document.createElement("label");
    l.innerHTML="Odaberi vreme:";
    l.className="Odaberi";
    red.appendChild(l);

    se = document.createElement("select");
    se.className="SelectZaDatum1";
    red.appendChild(se);
   
   this.listaRazlicitihDatuma.forEach(d=>{
     op = document.createElement("option");
    op.innerHTML=d;
    op.value=d;
    se.appendChild(op); })

    // Odaberi tip sedista
    red = this.crtajRed(host);
    l = document.createElement("label");
    l.innerHTML="Odaberi tip sedista:";
    l.className="Odaberi";
    red.appendChild(l);

    se = document.createElement("select");
    se.className="SelectZaTipSedista";
    red.appendChild(se);


    this.listaTipovaSedista.forEach(ts=>{
    op = document.createElement("option");
    op.innerHTML=ts;
    op.value=ts;
    se.appendChild(op);})
 
    // Dugme za dodavanje putnika
    red = this.crtajRed(host);
    let btnDodajPutnika = document.createElement("button");
    btnDodajPutnika.onclick=(ev)=>this.dodajPutnika(Ime.value,Prezime.value,Email.value,Jmbg.value,BrojLicneKarte.value,BrojTelefona.value);
    btnDodajPutnika.innerHTML="Zakazi let";
    btnDodajPutnika.className="Dugme";
    red.appendChild(btnDodajPutnika);
}

dodajPutnika(ime,prezime,email,jmbg,brojLicneKarte,brojTelefona)
{
    // Provera da li su ispravno uneti podaci
    if(ime===null || ime ==="" )
    {
        alert("Unesite ime");
        return;
    }
    if(prezime===null || prezime ==="" )
    {
        alert("Unesite prezime");
        return;
    }
    if(email===null || email ==="" )
    {
        alert("Unesite email");
        return;
    }
    if(jmbg===null || jmbg==="" )
    {
        alert("Unesite JMBG");
        return;
    }
    else
    {
        if(jmbg.length!=13)
        {
            alert("Neispravna vrednost je uneta za JMBG");
            return;
        }
    }
    if(brojLicneKarte===null || brojLicneKarte ==="" )
    {
        alert("Unesite broj licne karte");
        return;
    }
    else
    {
        if(brojLicneKarte.length!=9)
        {
            alert("Neispravna vrednost je uneta za broj licne karte");
            return;
        }
    }
    if(brojTelefona===null || brojTelefona ==="" )
    {
        alert("Unesite broj telefona ");
        return;
    }

    // Preuzimanje vrednosti za grad
    let optionEl = this.kont.querySelector(".SelectZaGrad1");
    var grad = optionEl.options[optionEl.selectedIndex].value; // ime grada

    // Preuzimanje vrednosti za datum
    optionEl = this.kont.querySelector(".SelectZaDatum1");
    var datum = optionEl.options[optionEl.selectedIndex].value; //datum


    // Preuzimanje vrednosti za tip sedista
    optionEl = this.kont.querySelector(".SelectZaTipSedista");
    var tipSedista = optionEl.options[optionEl.selectedIndex].value;

    //kreiranje putnika koji se prosledjuje preko body-ija
   
    var putnik=new Putnik(ime, prezime, email, jmbg, brojLicneKarte, brojTelefona, this.id );
  
    // ---------------------------- FETCH FJE -----------------------------

    // dodavanje leta
     var cenaSedista;
    if(tipSedista == "FC")
        cenaSedista= 50000;
    else if(tipSedista == "EC")
        cenaSedista= 10000;
    else if(tipSedista == "BC")
        cenaSedista=25000;

     var lett=new Let(tipSedista,cenaSedista,this.id);
    console.log(lett.tipSedista,lett.cenaSedista);
 
    fetch("https://localhost:5001/Putnik/DodatiPutnikaFromBody/"+ grad + "/" + datum,
    {
       method:"POST",
       headers: {"Content-Type": "application/json"},
        body:JSON.stringify({"ime":putnik.ime,"prezime":putnik.prezime,"email":putnik.email,
        "jmbg":putnik.jmbg,"brLicneKarte":putnik.brLicneKarte,"brTelefona":putnik.brTelefona})
    }).then(p=>{
        if(p.ok)
        {
            console.log("USLO U POST - dodati putnika")
           //this.listaPutnika.push(putnik);
           this.dodajPutnikaa(putnik);
        }
        else console.log(" NE ULAZI U POST - dodati putnika")

 

    fetch("https://localhost:5001/Let/DodatiLetFromBody/" + putnik.jmbg + "/" + grad + "/" + datum +"/"+ this.id,
    {
        method:"POST",
        headers: {"Content-Type": "application/json"},
        body:JSON.stringify({"tipSedista":lett.tipSedista, "cena":lett.cena})
    }).then(p=>{
        if(p.ok)
        {
            
          this.nadjiPutnike();
            console.log(" ULAZI U POST - dodati let")
            //this.listaLetova.push(lett);
            this.dodajLet(lett);
        }
        else alert("Ne postoji let za taj grad i taj datum!");
        this.listaLetova=[];
        fetch("https://localhost:5001/Let/PreuzmiLetoveZaAerodrom")
        .then(p=>{
            p.json().then(letovi=>{
                letovi.forEach(le => {
                    var l= new Let(le.tipSedista, le.cena, le.aerodrom.id); 
                    if(le.aerodrom.id == this.id)
                         this.listaLetova.push(l); })
            this.obrisiPrethodniSadrzajZaGraf();
            this.crtajGraf(this.kont);
    }) 
})
    });

  });  
  }

// --------------------------------- BRISANJE PUTNIKA SA LETA----------------------------------------
CrtajZaObrisiPutnika(host)
{
      //odaberi grad labela
      let red = this.crtajRed(host);
      let l = document.createElement("label");
      l.innerHTML="Odaberi grad: ";
      l.className="Odaberi";
      red.appendChild(l);

      //select za grad
      let se = document.createElement("select");
      se.className="SelectZaGrad2";
      red.appendChild(se);
      

      let op;
      this.listaRazlicitihDestinacija.forEach(d=>{
          op = document.createElement("option");
          op.innerHTML=d;
          op.value=d;
          se.appendChild(op);
      })

       //odaberi datum labela
      red = this.crtajRed(host);
      l = document.createElement("label");
      l.innerHTML="Odaberi datum:";
      l.className="Odaberi";
      red.appendChild(l);

      //select za datum
      se = document.createElement("select");
      se.className="SelectZaDatum2";
      red.appendChild(se);

      
    this.listaRazlicitihDatuma.forEach(d=>{
          op = document.createElement("option");
          op.innerHTML=d;
          op.value=d;
          se.appendChild(op);
      })

    //Unesite JMBG
    red = this.crtajRed(host);
    l =  document.createElement("label");
    l.innerHTML="Unesite JMBG:";
    red.appendChild(l);
    var JMBG = document.createElement("input");
    JMBG.type="string";
    JMBG.className="KlasaJMBG";
    red.appendChild(JMBG);

    //Dugme OBRISI
      red = this.crtajRed(host);
      let btnObrisiPutnika = document.createElement("button");
      btnObrisiPutnika.onclick=(ev)=>this.obrisiPutnika(JMBG.value);
      btnObrisiPutnika.innerHTML="Obrisi putnika sa leta";
      btnObrisiPutnika.className="Dugme";
      red.appendChild(btnObrisiPutnika);

}

obrisiPutnika(jmbg)
{
    if(jmbg===null || jmbg==="" )
    {
        alert("Unesite JMBG!");
        return;
    }
    else
    {
        if(jmbg.length!=13)
        {
            alert("Neispravna vrednost je uneta za JMBG!");
            return;
        }
    } 
    // Preuzimanje vrednosti za grad
    let optionEl = this.kont.querySelector(".SelectZaGrad2");
    var grad = optionEl.options[optionEl.selectedIndex].value; // ime grada

    // Preuzimanje vrednosti za datum
    optionEl = this.kont.querySelector(".SelectZaDatum2");
    var datum = optionEl.options[optionEl.selectedIndex].value; //datum

  fetch("https://localhost:5001/Let/IzbrisiLet/"+jmbg +"/"+ grad +"/"+datum +"/" + this.id,
  {
      method: "DELETE"
  }) .then(p=>
    {
        if(p.ok)
        {
            var teloTabele = this.obrisiPrethodniSadrzaj();
            this.nadjiPutnike();

            console.log("Obrisan putnik");
        }
        else alert("Ne postoji putnik sa unetim JMBG-om!");
 /////////////////////// ZA GRAF ////////////////////////////////
        this.listaLetova=[];
        fetch("https://localhost:5001/Let/PreuzmiLetoveZaAerodrom")
        .then(p=>{
            p.json().then(letovi=>{
                letovi.forEach(le => {
                    var l= new Let(le.tipSedista, le.cena, le.aerodrom.id); 
                    if(le.aerodrom.id == this.id)
                         this.listaLetova.push(l);

        })
         this.obrisiPrethodniSadrzajZaGraf();
        this.crtajGraf(this.kont);
    }) 
})
    });

}

// ---------------------------------- IZMENI TIP SEDISTA -------------------------------------

CrtajZaIzmeniTipSedista(host)
{
    //Unesite broj licne karte
    var red = this.crtajRed(host);
    var l =  document.createElement("label");
    l.innerHTML="Unesite broj licne karte:";
    red.appendChild(l);
    var brojLicneKarte = document.createElement("input");
    brojLicneKarte.type="string";
    brojLicneKarte.className="KlasaBrojLicneKarte";
    red.appendChild(brojLicneKarte);
    //Odaberite novi tip sedista
    red = this.crtajRed(host);
    l = document.createElement("label");
    l.innerHTML="Odaberi novi tip sedista:";
    l.className="Odaberi";
    red.appendChild(l);

    var se = document.createElement("select");
    se.className="SelectZaTipSedista1";
    red.appendChild(se);

    this.listaTipovaSedista.forEach(ts=>{
    var op = document.createElement("option");
    op.innerHTML=ts;
    op.value=ts;
    se.appendChild(op);
    })

    //query selector za tip sedista
    //let optionEl = this.kont.querySelector(".SelectZaTipSedista");
    //var tipSedista = optionEl.options[optionEl.selectedIndex].value;

    //console.log(tipSedista)
    //console.log(brojLicneKarte.value)
    //dugme za menjanje tipa sedista putniku
    red = this.crtajRed(host);
    let btnIzmeniTipSedista = document.createElement("button");
    btnIzmeniTipSedista.onclick=(ev)=>this.izmeniTipSedista(brojLicneKarte.value);
    btnIzmeniTipSedista.innerHTML="Izmeni tip sedista";
    btnIzmeniTipSedista.className="Dugme";
    red.appendChild(btnIzmeniTipSedista);
}

izmeniTipSedista(brojLicneKarte)
{

    if(brojLicneKarte===null || brojLicneKarte ==="" )
    {
        alert("Unesite broj licne karte");
        return;
    }
    else
    {
        if(brojLicneKarte.length!=9)
        {
            alert("Neispravna vrednost je uneta za broj licne karte");
            return;
        }
    }

     //query selector za tip sedista
     let optionEl = this.kont.querySelector(".SelectZaTipSedista1");
     var tipSedista = optionEl.options[optionEl.selectedIndex].value;
     

    fetch("https://localhost:5001/Putnik/IzmeniTipSedista/"+tipSedista+"/"+brojLicneKarte,
    {
        method:"PUT",
         headers:{
            "Content-Type":"application/json"
        }
    }).then(p=>{
        if(p.ok){
            this.nadjiPutnike();
            
          //  this.obrisiPrethodniSadrzajZaGraf();
           // this.crtajGraf(this.kont);

            console.log("Uspesno izmenjen tip sedista"+tipSedista);
        }
        else alert("Ne postoji putnik sa ovom licnom kartom!");
        ///////////////////////////////// ZA GRAF /////////////////////////////////
        this.listaLetova=[];
        fetch("https://localhost:5001/Let/PreuzmiLetoveZaAerodrom")
        .then(p=>{
            p.json().then(letovi=>{
                letovi.forEach(le => {
                    var l= new Let(le.tipSedista, le.cena, le.aerodrom.id); 
                    if(le.aerodrom.id == this.id)
                         this.listaLetova.push(l); })
         this.obrisiPrethodniSadrzajZaGraf();
        this.crtajGraf(this.kont);
    }) 
})
    });


}
}



        

 
