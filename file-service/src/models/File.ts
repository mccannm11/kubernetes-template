import {Document, Schema} from "mongoose";
import * as mongoose from "mongoose";
import {Guid} from "../types";

const fileSchema = new Schema({
    id: {type: String, required: true, unique: true},
    name: {type: String, required: true}
})


interface IFile extends Document {
    id: Guid
    name: string
}

const File = mongoose.model<IFile>('File', fileSchema)

export {File}