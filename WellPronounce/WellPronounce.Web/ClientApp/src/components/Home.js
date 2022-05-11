import React, { Component, useState } from 'react';
import { CopyToClipboard } from 'react-copy-to-clipboard';
import Player from "./Player";
import createRecorder from "react-simple-recorder";
const Recorder = createRecorder(React);

export class Home extends Component {
    static displayName = Home.name;
    constructor(props) {
        super(props);

        this.state = {
            value: "",
            copied: false,
            status: 1,
            standardTextInput: "",
            customTextInput: ""
        };

        this.handleStandardChange = this.handleStandardChange.bind(this);
        this.handleCustomChange = this.handleCustomChange.bind(this);
    }


    handleStandardChange(event) {
        this.setState({ standardTextInput: event.target.value });
   
    }

    handleCustomChange(event) {
        this.setState({ customTextInput: event.target.value });
    }

    radioHandler = (status) => {
        this.setState({ status });
        this.setState({ value: "" });
        this.setState({ standardTextInput: "" });
        this.setState({ customTextInput: "" });
    };

    convertOnClick = () => {
        document.getElementById('modal-root').style.filter = 'blur(5px)'
        var payload = {
            text: this.state.standardTextInput,
            language: "English",
            processType: "Standard"
        };

        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        };

        const response = fetch('/api/TextToSpeech/StandardProcess', requestOptions).then(r => r.json()).then(res => {
            if (res) {
                this.setState({
                    value: res.path
                });
                document.getElementById('modal-root').style.filter = 'blur(0px)'
            }
        });

    }

    customProcess = (blob) => {
        console.log(blob);

        var payload = {
            text: this.state.customTextInput,
            language: "English",
            processType: "NonStandard",
            audioFile : blob
        };

       

        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        };

        const response = fetch('/api/TextToSpeech/CustomProcess', requestOptions).then(r => r.json()).then(res => {
            if (res) {
                this.setState({
                    value: res.path
                });
            }
        });


        //let data = new FormData();

        //data.append('text', this.state.customTextInput);
        //data.append('language', "English");
        //data.append('processType', "Custom");
        //data.append('audioFile', blob, "audioFile.wav");

        //const config = {
        //    headers: { 'content-type': 'multipart/form-data' }
        //}
        //axios.post('/api/TextToSpeech/CustomProcess', data, config);


    }

    render() {
        const { status } = this.state;

        const myDivstyle = {
            padding: "3px",
        };

        return (
            <div id="modal-root" className="container">
                {/*Standard or Custom Type Selection*/}
                <div className="row">
                    <div className="col-sm-5">
                        <p>
                            <b>Select the option based on your prefered approach</b>
                        </p>
                    </div>
                    <div className="col-sm-7">
                        <div className="form-check-inline">
                            <label className="form-check-label" for="standardradio">
                                <input type="radio" className="form-check-input" id="standardradio" name="optradio" value="Standard" checked={status === 1} onClick={(e) => this.radioHandler(1)} /><b>Standard</b>
                            </label>
                        </div>
                        <div className="form-check-inline">
                            <label className="form-check-label" for="customradio">
                                <input type="radio" className="form-check-input" id="customradio" name="optradio" value="NonStandard" checked={status === 2} onClick={(e) => this.radioHandler(2)} /><b>NonStandard</b>
                            </label>
                        </div>
                    </div>
                </div>
                <br />

                {/*Standard Section*/}
                {status === 1 &&
                    <div id="standardDiv">

                        {/* Input Text*/}
                        <div className="row">
                            <div className="col-sm-1" style={myDivstyle}>
                                <b>Text</b>
                            </div>
                            <div className="col-sm-3" style={myDivstyle}>
                                <textarea type="text" className="form-control" value={this.state.standardTextInput} onChange={this.handleStandardChange} id="standardTextInput" />
                            </div>
                            <div className="col-sm-3" style={myDivstyle}>
                                <input type="text" className="form-control" id="standardLanguageInput" disabled value="English" />
                            </div>
                            <div className="col-sm-3" style={myDivstyle}>
                                <button className="btn btn-dark" id="standardPlay" onClick={this.convertOnClick}>Play</button>
                            </div>
                            <div className="col-sm-2" style={myDivstyle}>
                                {/*<Player*/}
                                {/*    url={this.state.value}*/}
                                {/*/>*/}
                            </div>
                        </div>
                        <br />

                        {/* Output Link*/}
                        <div className="row">
                            <div className="col-sm-1" style={myDivstyle}><b>Link</b></div>
                            <div className="col-sm-9" style={myDivstyle}>
                                <input type="text" className="form-control" id="standardDataOutput" value={this.state.value}
                                    onChange={({ target: { value } }) => this.setState({ value, copied: false })} disabled />
                            </div>
                            <div className="col-sm-2" style={myDivstyle}>
                                <CopyToClipboard text={this.state.value}
                                    onCopy={() => this.setState({ copied: true })}>
                                    <button className="btn btn-success"><i className="fas fa-copy"></i></button>
                                </CopyToClipboard>

                                {this.state.copied ? <span style={{ color: 'red' }}> Copied.</span> : null}
                            </div>
                        </div>

                    </div>
                }

                {/*Custom Section*/}
                {status === 2 &&
                    <div id="customDiv">
                        <div className="row">
                            <div className="col-sm-1" style={myDivstyle}>
                                <b>Text</b>
                            </div>
                            <div className="col-sm-3" style={myDivstyle}>
                            <textarea type="text" className="form-control" value={this.state.customTextInput} onChange={this.handleCustomChange} id="customTextInput" />
                            </div>
                            <div className="col-sm-3" style={myDivstyle}>
                                <input type="text" className="form-control" id="customLanguageInput" disabled value="English" />
                            </div>
                            <div className="col-sm-5" style={myDivstyle}>
                                <Recorder
                                    containerClassName="my-recorder-container"
                                    Stop={<div>Stop</div>}
                                    Play={<p>Play</p>}
                                    Pause={<button>Pause</button>}
                                    Record={<div>Record</div>}
                                    Send={<div>Upload</div>}
                                    onSend={(blobUrl, blob) => {
                                        this.customProcess(blob);
                                    }}
                                />
                            </div>
                        </div>

                        <div className="row">
                            <div className="col-sm-1" style={myDivstyle}><b>Output</b></div>
                            <div className="col-sm-9" style={myDivstyle}>
                                <textarea type="text" className="form-control" id="customDataOutput" value={this.state.value}
                                    onChange={({ target: { value } }) => this.setState({ value, copied: false })} disabled />
                            </div>
                            <div className="col-sm-2" style={myDivstyle}>
                                <CopyToClipboard text={this.state.value}
                                    onCopy={() => this.setState({ copied: true })}>
                                    <button className="btn btn-success"><i className="fas fa-copy"></i></button>
                                </CopyToClipboard>

                                {this.state.copied ? <span style={{ color: 'red' }}> Copied.</span> : null}
                            </div>
                        </div>
                    </div>
                }
            </div>
        );
    }
}
