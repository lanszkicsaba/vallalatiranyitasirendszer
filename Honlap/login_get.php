<?php
include 'dbconnect.php';
session_start();
?>
<?php
$conn = MySQLServerConnecter(); //kapcsolat megnyitása

$query = "SELECT Username, Password FROM honlapusers WHERE Username='" . $_POST["username"] . "'"; //lekéri a jelszót és a felhasználó adatait
$s = $conn->query($query); //lekérés futatás
$conn->close(); // kapcsolat lezárása

$row = $s->fetch_assoc(); //adatatok felbontása

$password = md5($_POST["psw"]); //beírt jelszó md5 hash-é alakítása
//jelszó ellenőrzés
if ($password == $row["Password"]) {
    $_SESSION["login"] = "TRUE"; //belépetté alakítása
    $_SESSION["user"] = $_POST["username"]; //felhasználó név átadása
    echo '<meta http-equiv="refresh" content="0; URL=index.php">'; //visszadobás index.php-re
} else {
    //nem megfelelő értesítés
    echo '<script>alert("Nem megfelelő a Jelszó vagy a Login név");</script>';
    echo '<meta http-equiv="refresh" content="0; URL=login.php">'; //visszadobás login.php-re
}
