<?php

session_start();
?>
<?php

$_SESSION["login"] = "FALSE"; //belépés átállítása nem re
$_SESSION["user"] = "nincs belepve"; //felhasználói névátállítása
//nincs bejelentkezve visszadobja az index.php-re
echo '<meta http-equiv="refresh" content="0; URL=index.php">';
