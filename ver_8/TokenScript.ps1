# $TenantId = ""
# $ClientId = ""
# $Secret = ""

# $uri = "https://localhost:7172/customer/helloCustomers"

# $body = @{
#     grant_type = "client_credentials"
#     client_id = $ClientId
#     client_secret = $Secret
#     resource = "https://graphs.mircosoft.com"
# }


# $resp = Invoke-RestMEthod -Method

# Invoke-RestMethod -Method GET -ContentType "application/json" -Uri "https://localhost:7172/customer/helloCustomers"


# try {
#     Invoke-RestMethod -Uri "https://localhost:7172/generate-Tokens" -Method GET -Body $body  -ContentType  'application/json'
# } catch {
#     $response = $_.Exception.Response
#     if ($response.StatusCode -eq [System.Net.HttpStatusCode]::BadRequest) {
#         $stream = $response.GetResponseStream()
#         $reader = New-Object System.IO.StreamReader($stream)
#         $reader.BaseStream.Position = 0
#         $reader.DiscardBufferedData()
#         $responseBody = $reader.ReadToEnd();
#         Write-Warning $responseBody
#     }
#     throw
# }




# $Cred = Invoke-RestMethod -Method GET -ContentType "application/json" -Uri "https://localhost:7172/generate-Tokens"
#     $headers = @{
#         Authorization = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2ODYyMTMwODksImV4cCI6MTY4NjIxMzI0NCwiaXNzIjoiYWJoaXNoZWsiLCJhdWQiOiJhYmhpc2hlayJ9.6XnVyWZOvcQifvn85ELmKAUHtlQ8ccdOiQ8caz1U7Z4" 
#    }

# Invoke-RestMethod -Method GET -ContentType "application/json" -Uri "https://localhost:7172/customers/helloCustomers" -Headers $headers.ToString()




# $Params = @{
# 	Uri = "https://localhost:7172/customer/helloCustomers"
# 	#Authorization = "Bearer" + $Cred
# #	Credential = $Cred
#     $headers = @{
#         Authorization = "Bearer " + $Cred
#    } 
    
# }

# Invoke-RestMethod @Params




# $headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
# $token = Invoke-RestMethod -Method GET -ContentType "application/json" -Uri "https://localhost:7172/generate-Tokens"
# $headers.Add("Authorization","Bearer" + $Token)


# Invoke-RestMethod -Method GET -ContentType "application/json" -Uri "https://localhost:7172/customer/helloCustomers" -Headers $headers 
# $response | ConvertTo-Json



$user= Read-Host -Prompt "Enter your User Id"
$pass = Read-Host -Prompt "Enter your Password"
$url = "https://localhost:7172/customer/validCustomer/$user/$pass"
# echo $url

$token = Invoke-RestMethod -Method GET -ContentType "application/json" -Uri "$url"

# echo $token[0]

$headers = @{
    "Authorization" = "Bearer " + $token[0]
   
}
Invoke-RestMethod -ContentType "application/json" -Uri "https://localhost:7172/customer/GetAllCustomerDetails" -Method GET -Headers $headers 

