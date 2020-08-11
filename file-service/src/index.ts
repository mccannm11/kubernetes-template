import express from "express"

const portNumber = process.env.NODE_PORT || 3000

const app = express()

app.get("/", (req, res) => {
    res.send("File service response")
})

app.listen(portNumber, () => {
    console.log(`File service is now listening on port ${portNumber}`)
})
