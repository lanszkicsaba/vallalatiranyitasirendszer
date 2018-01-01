<?php
session_start();
if (isset($_REQUEST['idd']))
{
	$nulla=array_search($_REQUEST['idd'],$_SESSION['cart2']);
	if($nulla!==false)
    unset($_SESSION['cart2'][$nulla]);
    
	$key=array_search($_REQUEST['idd'],$_SESSION['cart2']);
    if($key!==false)
    unset($_SESSION['cart2'][$key]);
    $_SESSION["cart2"] = array_values($_SESSION["cart2"]);	
	
} 
?>
