<?php

include 'dbconnect.php';

session_start();
?>

<?php

//ha bevan lépve
if (count($_SESSION) > 0 && $_SESSION["login"] == "TRUE") {
    $conn = MySQLServerConnecter(); //adatbázis kapcsolat létrehozása
    //lekérés a felhasználó adatait
    $query = "SELECT Id, Fullname, Address, Phonenumer, Taxnumber FROM honlapusers WHERE Username='" . $_SESSION['user'] . "'";
    //lekérés végre hajtása
    $s = $conn->query($query);
    $conn->close(); //kapcsolat lezárása

    $row = $s->fetch_assoc(); //sorok felbontása

    $writer = new XMLWriter(); //XML író
    $writer->openUri('rendeles.xml'); //XML neve
    $writer->startDocument('1.0', 'UTF-8'); //A Formátum fejlécs
    $writer->startElement('rendeles');      //levél létrehozása
    $writer->startElement("rendeloadatai"); //Melléklevél létrehozása
    $writer->writeElement('nev', $row["Fullname"]); //név adat
    $writer->writeElement('cim', $row["Address"]);  //cím adat
    $writer->writeElement('telefonszam', $row["Phonenumer"]); //Telefonszám
    $writer->writeElement('adoszam', $row["Taxnumber"]); //adószám
    $writer->endElement(); //Melléklevél lezárása
    $writer->startElement("termekekadatai"); //melléklevél létrehozása

    $conn = MySQLServerConnecter(); //kapcsolat megnyitás
    date_default_timezone_set("Europe/Budapest"); //időzóna
    $queryinsert = "INSERT INTO rendelesek VALUES (NULL,'" . date("Y.m.d h:i:sa") . "','" . $row["Id"] . "');"; //rendelés azonosítók beszúrása
    $conn->query($queryinsert); //query végrehajtása
    $queryget = "SELECT max(id) as id FROM rendelesek;"; //query
    $s = $conn->query($queryget); //query lefutatása
    $conn->close(); //kapcsolat lezárása

    $rendelesid = $s->fetch_assoc(); //rendelési azonosító
    
    for ($index = 0; $index < count($_SESSION["cart2"]); $index++) {

        $conn = MySQLServerConnecter(); //kapcsolat megnyitás

        $query = "SELECT id, termeknev, ar FROM termekek WHERE id=" . $_SESSION["cart2"][$index]; //lekéri az i. megrendelni kívánt termék adatait

        $s = $conn->query($query); //query lefutatása
        $conn->close(); //kapcsolat lezárása

        $row = $s->fetch_assoc(); //sorok felbontása
        if (isset($_POST[$index])) {
            $writer->writeElement('termekneve', $row["termeknev"]); //termékneve
            $writer->writeElement('mennyiseg', $_POST[$index] . ' db'); //termék mennyisége
            $writer->writeElement('ara', $_POST[$index] * $row["ar"] . ' Ft'); //ára számítva

            $conn = MySQLServerConnecter(); //kapcsolat megnyitás
            $queryinsert = "INSERT INTO rendeles_adatok VALUES (NULL,'" . $rendelesid["id"] . "','" . $row["id"] . "','" . $_POST[$index] . "');"; //megrendelt termékek feltöltése
            $conn->query($queryinsert); //query lefutatása
            $conn->close(); //kapcsolat lezárása
        }
    }
    $writer->endElement(); //melléklevél lezárás
    $writer->endElement(); //levéllezárás
    $writer->endDocument(); //dokumentum lezárás
    $writer->flush(); //fájl kiírása
    echo '<p>Köszönjük megrendelését! Hamarosan szállítjuk a megrendelését!</p>';
    $_SESSION["cart2"] = array(); //lista ürítése
} else {
    //nincs bejelentkezve visszadobja az index.php-re
    echo '<meta http-equiv="refresh" content="0; URL=index.php">';
}
?>
