import express, { Request } from "express"
import S3, {
  GetObjectOutput,
  GetObjectRequest,
  ListObjectsOutput,
  ListObjectsRequest,
} from "aws-sdk/clients/s3"
import { config } from "./config"
import { AWSError } from "aws-sdk/lib/error"
import { Guid } from "./types"
const portNumber = process.env.NODE_PORT || 3000

const app = express()

const s3 = new S3({
  secretAccessKey: config.SecretAccessKey,
  accessKeyId: config.AccessKeyId,
  apiVersion: "2006-03-01",
})

type AwsCallback<TOutput> = (err: AWSError, data: TOutput) => void

/**
 * Many of these s3 functions have different signatures, might need to make this more dynamic?
 */
const makeS3Promise = <TParams, TOutput>(
  s3Func: (params: any, callback: AwsCallback<TOutput>) => void
) => (params: TParams) => {
  return new Promise<TOutput>((resolve, reject) =>
    s3Func.bind(s3)(params, (error, data) => {
      if (error) {
        reject(error)
      } else {
        resolve(data)
      }
    })
  )
}

const getObject = makeS3Promise<GetObjectRequest, GetObjectOutput>(s3.getObject)
const listObjects = makeS3Promise<ListObjectsRequest, ListObjectsOutput>(
  s3.listObjects
)

/**
 * Get a single object
 */
type GetFileGuidParams = {
  fileGuid: Guid
}
app.get("/:fileGuid", async (req: Request<GetFileGuidParams>, res) => {
  // list all files
  
  const params = {
    Bucket: config.BucketName,
    Key: req.params.fileGuid,
  }

  try {
    const result = await getObject(params)
    res.write(JSON.stringify(result))
  } catch (error) {
    res.write(JSON.stringify(error))
  }

  res.end()
})

/**
 * Does the user have read permissions?
 * Write a record of the read access
 * Return files depending on file extension
 * .json -> json of all file data
 * .zip -> archive files and reurn download
 * for large files -> kick job to compress
 * files and email user when complete
 */
app.get("/", async (req, res) => {
  const params = {
    Bucket: config.BucketName,
  }
  
  try {
    const result = await listObjects(params)
    console.log("List objects success")
    res.write(JSON.stringify(result))
  } catch (error) {
    res.write(JSON.stringify(error))
  }
  res.end()
})

/**
 * Does the user have write permissions
 * Log the write
 * Write file metadata
 * Return the guid that was assigned if one was not provided in the request
 */
app.post("/", (req, res) => {})

/**
 * Patch - update file
 * Does the user have update permissions?
 * Record in audit log who updated the file
 * Update the file in s3
 * Do I keep a history of all files ever changed?
 */
app.patch("/:fileGuid", (req, res) => {})

/**
 * Does the user have delete permissions?
 *
 */
app.delete("/:fileGuid", (req, res) => {})

app.listen(portNumber, () => {
  console.log(`File service is now listening on port ${portNumber}`)
})
