<?php

include 'dbconnect.php';
session_start();
?>

<?php

if (count($_SESSION) > 0 && $_SESSION["login"] == "TRUE") {
    $conn = MySQLServerConnecter();

    $query = "SELECT Fullname, Address, Phonenumer  FROM honlapusers WHERE Username='" . $_SESSION['user'] . "'";
    $s = $conn->query($query);
    $conn->close();

    $row = $s->fetch_assoc();

    $writer = new XMLWriter();
    $writer->openUri('rendeles.xml');
    $writer->startDocument('1.0', 'UTF-8');
    $writer->startElement('rendeles');
    $writer->startElement("rendeloadatai");
    $writer->writeElement('nev', $row["Fullname"]);
    $writer->writeElement('cim', $row["Address"]);
    $writer->writeElement('telefonszam', $row["Phonenumer"]);
    $writer->endElement();
    $writer->startElement("termekekadatai");

    for ($index = 0; $index < count($_SESSION["cart2"]); $index++) {
        $conn = MySQLServerConnecter();

        $query = "SELECT termeknev, ar FROM termekek WHERE id=" . $_SESSION["cart2"][$index];
        $s = $conn->query($query);
        $conn->close();

        $row = $s->fetch_assoc();

        $writer->writeElement('termekneve', $row["termeknev"]);
        $writer->writeElement('mennyiseg', $_POST[$index] . ' db');
        $writer->writeElement('ara', $_POST[$index] * $row["ar"] . ' Ft');
    }
    $writer->endElement();
    $writer->endElement();
    $writer->endDocument();
    $writer->flush();
    echo '<p>Köszönjük megrendelését! Hamarosan szállítjuk a megrendelését!</p>';
    $_SESSION["cart2"]=array();
} else {
    echo '<meta http-equiv="refresh" content="0; URL=index.php">';
}
?>