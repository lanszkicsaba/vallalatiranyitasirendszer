<?php
include 'dbconnect.php';
session_start();
?>
<?php
$conn = MySQLServerConnecter(); //kapcsolat megnyitása

$s=$conn->prepare("SELECT Username, Password FROM honlapusers WHERE Username=?"); //MYSQL statement létrehozása
$s->bind_param("s", $_POST["username"]); //Változók hozzárendelése a statementhez

$s->execute(); //Statement futtatása
$r=$s->get_result(); //Lekérdezés eredményének hozzárendelése a $resulthoz

$conn->close(); // kapcsolat lezárása

$row = $r->fetch_assoc(); //adatatok felbontása

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
