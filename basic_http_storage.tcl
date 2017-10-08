package require fcgi
package require sqlite3
package require rest

fcgi Init
set sock [fcgi OpenSocket :8000]
set req [fcgi InitRequest $sock 0]
set data {}

proc url-decode str {
	# rewrite "+" back to space
	# protect \ from quoting another '\'
	set str [string map [list + { } "\\" "\\\\"] $str]

	# prepare to process all %-escapes
	regsub -all -- {%([A-Fa-f0-9][A-Fa-f0-9])} $str {\\u00\1} str

	# process \u unicode mapped chars
	return [subst -novar -nocommand $str]
}

while {1} {
	fcgi Accept_r $req
	#get the requested page
	set pd [fcgi GetParam $req]
	set request_str [dict get $pd REQUEST_URI]
	set query_params [rest::parameters $request_str]
	if [dict exists $query_params data] {
		set data [url-decode [dict get $query_params data]]
	}
	
	#output the page
	set C "Status: 200 OK\n"
	append C "Content-Type: "
	append C "text/html"
	append C "\r\n\r\n"
	append C $data
	fcgi PutStr $req stdout $C
	fcgi SetExitStatus $req stdout 0
	fcgi Finish_r $req
}