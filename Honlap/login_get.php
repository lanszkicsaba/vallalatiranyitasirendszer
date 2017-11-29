<?php
include 'dbconnect.php';
session_start();
?>
<?php
$conn = MySQLServerConnecter();

$query = "SELECT Username, Password FROM honlapusers WHERE Username='" . $_POST["username"] . "'";
$s = $conn->query($query);
$conn->close();

$row = $s->fetch_assoc();

$password = md5($_POST["psw"]);

if ($password == $row["Password"]) {
    $_SESSION["login"] = "TRUE";
    $_SESSION["user"] = $_POST["username"];
    echo '<meta http-equiv="refresh" content="0; URL=index.php">';
} else {
    echo '<script>alert("Nem megfelelő a Jelszó vagy a Login név");</script>';
    echo '<meta http-equiv="refresh" content="0; URL=login.php">';
}
