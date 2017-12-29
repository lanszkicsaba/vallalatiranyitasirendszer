<?php

session_start();
?>
<?php

$_SESSION["login"] = "FALSE";
$_SESSION["user"] = "nincs belepve";
echo '<meta http-equiv="refresh" content="0; URL=index.php">';
